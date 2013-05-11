using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Xml.Serialization;

namespace MSTestResultViewer.Consol
{
    class Program
    {
        public static TestRun testRun;


        #region "Variables"
        const int SLEEP_EXECUTION_TIME = 200;
        const string TAG_UNITTEST = "UnitTest";
        const string TAG_ERRORINFO = "ErrorInfo";

        const string TABLE_TESTMETHOD = "TestMethod";
        const string TABLE_TESTRUN = "TestRun";
        const string TABLE_UNITTESTRESULT = "UnitTestResult";
        const string TABLE_TIMES = "Times";

        const string COLUMN_CLASSNAME = "className";
        const string COLUMN_NAME = "name";
        const string COLUMN_RUNUSER = "runUser";
        const string COLUMN_MESSAGE = "Message";
        const string COLUMN_STACKTRACE = "StackTrace";
        const string COLUMN_COUNTERS = "Counters";
        const string COLUMN_TOTAL = "total";
        const string COLUMN_PASSED = "passed";
        const string COLUMN_FAILED = "failed";
        const string COLUMN_INCONCLUSIVE = "inconclusive";
        const string COLUMN_TESTNAME = "testName";
        const string COLUMN_OUTCOME = "outcome";
        const string COLUMN_DURATION = "duration";
        const string COLUMN_CREATION = "creation";
        const string COLUMN_CODEBASE = "codebase";

        const string ATTRIBUTE_TESTMETHODID = "TestMethodID";
        const string ATTRIBUTE_ID = "id";
        const string ATTRIBUTE_TESTID = "testId";
        const string FILENAME_TRX = "MSTestResult.trx";

        private static TestEnvironmentInfo testEnvironmentInfo;
        private static TestResultSummary testResultSummary;
        private static List<TestProjects> testProjects;

        static string projectChartDataValue = "";
        static string classChartDataValue = "";
        static string classChartDataText = "";
        static string methoChartDataValue = "";
        static string methoChartDataText = "";
        static string methoChartDataColor = "";
        static string folderPath = "";
        static string trxFilePath = "";

        static string MSTestExePathParam = "";
        static string TestContainerFolderPathParam = "";
        static string DestinationFolderParam = "";
        static string LogFileParam = "";
        static string HelpFileParam = "";
        #endregion

        #region "Properties"
        public static string Title
        {
            get { return GetValue<AssemblyTitleAttribute>(a => a.Title); }
        }
        public static string Version
        {
            get { return "1.0"; } //GetValue<AssemblyVersionAttribute>(a => a.Version); }
        }
        static string GetValue<T>(Func<T, string> getValue) where T : Attribute
        {
            T a = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
            return a == null ? "" : getValue(a);
        }
        #endregion

        #region "Program Startup"
        static void Main(string[] args)
        {
            Console.WriteLine(string.Format("{0} [Version {1}]\n", Title, Version));
            Console.WriteLine(string.Format("Copyright (c) 2012 Saffroze Technology Pvt. Ltd.\n"));
            if (RecognizeParameters(args))
            {
                try
                {
                    Directory.CreateDirectory(DestinationFolderParam);
                    string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    folderPath = Path.Combine(Path.Combine(appPath, "Data"));
                    Directory.CreateDirectory(folderPath);

                    if (MSTestExePathParam != "")
                    {
                        GenerateTRXFile(MSTestExePathParam, TestContainerFolderPathParam);
                    }
                    Console.WriteLine(string.Format("Start TRX file transformation....\n"));

                    parseXml(trxFilePath);

                    Transform(trxFilePath);

                    Console.WriteLine(string.Format("Transfering files at \"{0}\"\n", DestinationFolderParam));
                    CopyFilesWithSubFolders(Path.Combine(appPath, "Images"), Path.Combine(DestinationFolderParam, "Images"), true);
                    CopyFilesWithSubFolders(Path.Combine(appPath, "Javascripts"), Path.Combine(DestinationFolderParam, "Javascripts"), true);
                    CopyFilesWithSubFolders(Path.Combine(appPath, "Library"), Path.Combine(DestinationFolderParam, "Library"), true);
                    CopyFilesWithSubFolders(Path.Combine(appPath, "RGraph"), Path.Combine(DestinationFolderParam, "RGraph"), true);
                    CopyFilesWithSubFolders(Path.Combine(appPath, "Styles"), Path.Combine(DestinationFolderParam, "Styles"), true);
                    CopyFilesWithSubFolders(Path.Combine(appPath, "Data"), Path.Combine(DestinationFolderParam, "Data"), true, "*.js");
                    CopyFilesWithSubFolders(Path.Combine(appPath, "Pages"), DestinationFolderParam, true);
                    Console.WriteLine(string.Format("File Transfer completed\n"));

                }
                catch (Exception ex)
                {
                    Console.WriteLine("-------------------------ERROR-------------");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine(string.Format("Error: {0}", ex.StackTrace));
                    Console.WriteLine("-------------------------------------------");
                }
            }
            else
            {
                DisplayCommandLineHelp();
            }
        }

        private static void parseXml(string trxFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestRun));

            FileStream stream = new FileStream(trxFilePath, FileMode.Open);

            testRun = (TestRun)serializer.Deserialize(stream);

            stream.Close();
        }
        #endregion

        #region "Private Methods"
        private static void Transform(string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(fileName))
            {
                xmlDoc.Load(fileName);
                XmlNodeList list = xmlDoc.GetElementsByTagName(TAG_UNITTEST);
                foreach (XmlNode node in list)
                {
                    XmlAttribute newAttr = xmlDoc.CreateAttribute(ATTRIBUTE_TESTMETHODID);
                    newAttr.Value = node.Attributes[ATTRIBUTE_ID].Value;
                    node.ChildNodes[1].Attributes.Append(newAttr);
                }

                list = xmlDoc.GetElementsByTagName(TAG_ERRORINFO);
                foreach (XmlNode node in list)
                {
                    XmlAttribute newAttr = xmlDoc.CreateAttribute(ATTRIBUTE_TESTMETHODID);
                    newAttr.Value = (((node).ParentNode).ParentNode).Attributes[ATTRIBUTE_TESTID].Value;
                    node.Attributes.Append(newAttr);
                }

                //xmlDoc.Save(fileName);

                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xmlDoc));

                if (ds != null && ds.Tables.Count >= 4)
                {
                    Console.WriteLine(string.Format("Start gathering test environment information...\n"));
                    System.Threading.Thread.Sleep(SLEEP_EXECUTION_TIME);
                    SetTestEnvironmentInfo(ds);

                    Console.WriteLine(string.Format("Start gathering test result summary...\n"));
                    System.Threading.Thread.Sleep(SLEEP_EXECUTION_TIME);
                    SetTestResultSummary(ds);

                    Console.WriteLine(string.Format("Start gathering test classes methods information...\n"));
                    System.Threading.Thread.Sleep(SLEEP_EXECUTION_TIME);
                    SetTestClassMethods(ds);

                    if (testProjects.Count >= 1)
                    {
                        Console.WriteLine(string.Format("Start transforming test result into html...\n"));
                        System.Threading.Thread.Sleep(SLEEP_EXECUTION_TIME);

                        CreateTestHierarchy();

                        CreateTestResultTable();

                        CreateTestResultChart();

                        Console.WriteLine(string.Format("TRX file transformation completed successfully. \nFile generated at: \"{0}.htm\"\n", trxFilePath));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("No test cases are available for test\n"));
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("No test cases are available for test\n"));
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine(string.Format("Test Result File (.trx) not found at \"" + trxFilePath + "\"!\n"));
                Console.ReadLine();
            }
        }
        private static void SetTestEnvironmentInfo(DataSet ds)
        {
            //TODO: add validation for size >0
            var codebase = testRun.TestDefinitions[0].TestMethod[0].codeBase;
            var runUser = testRun.runUser;
            var trxfile = testRun.name;
            var cre = testRun.Times[0].creation;
            var creationDate= Convert.ToDateTime(cre);
            int idxattherate = trxfile.IndexOf("@") + 1;
            int idxspace = trxfile.IndexOf(" ");
            var machine = trxfile.Substring(idxattherate, idxspace - idxattherate);

        
            testEnvironmentInfo = new TestEnvironmentInfo()
            {
                MachineName = machine,
                OriginalTRXFile = trxfile,
                TestCodebase = codebase,
                UserName = runUser,
                Timestamp = creationDate,
            };
        }
        private static void SetTestResultSummary(DataSet ds)
        {
            //TODO: check if there is size >0
            var counter = testRun.ResultSummary[0].Counters[0];
           

            var st = testRun.Times[0].start;
            var startDate = Convert.ToDateTime(st);
            var end = testRun.Times[0].finish;
            var endDate = Convert.ToDateTime(end);
            var durationTime = endDate - startDate;
            var time = durationTime.ToString();
             

            testResultSummary = new TestResultSummary()
            {
                Total = Convert.ToInt16(counter.total),
                Passed = Convert.ToInt16(counter.passed),
                Failed = Convert.ToInt16(counter.failed),
                Ignored = Convert.ToInt16(counter.inconclusive),
                Duration = time,
                TestEnvironment = testEnvironmentInfo
            };
        }
        private static void SetTestClassMethods(DataSet ds)
        {
            HashSet<string> projects =new HashSet<string>();
            var comma = ", ";
            testProjects = new List<TestProjects>();
            var testDef= testRun.TestDefinitions;

            foreach (var unittest in testDef)
            {
                
                var classFullDetails = unittest.TestMethod[0].className; //Should be only one TestMethod under UnitTest tag
                int start =classFullDetails.IndexOf(comma);
                var className = classFullDetails.Substring(0, start);
                var temp = classFullDetails.Substring(start + 1);
                int end =temp.IndexOf(comma)-1 ;

                var projectName = classFullDetails.Substring(start + comma.Length, end).Trim();
                if (projects.Contains(projectName))
                {
                     
                    var p = new TestProjects() { Name = projectName };
                    var found = testProjects.Find(proj => proj.Name.Equals(p.Name));
                    var tcm = new TestClassMethods
                                  {
                                      Name = unittest.name,
                                      Id = unittest.id,
                                      Category = unittest.TestCategory[0].TestCategory
                                  };
                    if (found.Classes == null)
                    {
                        found.Classes = new List<TestClasses>();
                        var tc = new TestClasses {Name = className, Methods = new List<TestClassMethods>()};
                        tc.Methods.Add(tcm);
                        found.Classes.Add(tc);
                    }
                    else
                    {
                        var classTemp =new TestClasses() { Name = className };
                        var classFound = found.Classes.Find(classInList => classInList.Name.Equals(classTemp.Name));
                        if (classFound == null)
                        {
                            var tc= new TestClasses() {Name = className};
                            tc.Methods.Add(tcm);
                            found.Classes.Add(tc);
                        }
                        //else
                        //{
                        // class alredy in the list of the proejcts.
                        //}

                    }
                   

                }
                else
                {
                    projects.Add(projectName);
                    var testproject = new TestProjects() { Name = projectName };
                    testproject.Classes = new List<TestClasses>();
                    testproject.Classes.Add(new TestClasses() { Name = className });
                    testProjects.Add(testproject);
                    
                }
              
            }

            //foreach (var projName in projects)
            //{
            //    testProjects.Add(new TestProjects() { Name = projName });
            //}


            //DataView view = new DataView(ds.Tables[TABLE_TESTMETHOD]);
            //DataTable distinctValues = view.ToTable(true, COLUMN_CLASSNAME);
            //char[] delimiters = new char[] { ',' };
    

            ////Iterate through all the projects for getting its classes
            //foreach (TestProjects project in testProjects)
            //{
            //    DataRow[] classes = distinctValues.Select(COLUMN_CLASSNAME + " like '% " + project.Name + ", %'");
            //    if (classes != null && classes.Count() > 0)
            //    {
            //        project.Classes = new List<TestClasses>();
            //        foreach (DataRow dr in classes)
            //        {
            //            string _class = dr[COLUMN_CLASSNAME].ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            //            project.Classes.Add(new TestClasses() { Name = _class });
            //        }
            //    }
            //}


            Int32 passed = 0;
            Int32 failed = 0;
            Int32 ignored = 0;
            TimeSpan projectTotalDuration = TimeSpan.Parse("00:00:00.00");

            foreach (var testReuslt in testRun.Results)
            {
                var duration= testReuslt.duration;
                var testOutcome= testReuslt.outcome;

                switch (testOutcome.ToUpper())
                {
                    case "PASSED":
                        passed++;
                        break;
                    case "FAILED":
                        failed++;
                        break;
                    default:
                        ignored++;
                        break;
                }
                projectTotalDuration+= TimeSpan.Parse(duration);

            }


            //Iterate through all the projects and then classes to get test methods details
            //TODO: support more than one test project[each project should have totla pass,total failed,total ignore ,and total tests,and total time of all the tests]
            //TimeSpan durationProject = TimeSpan.Parse("00:00:00.00");
            //foreach (TestProjects _project in testProjects)
            //{
            //    Int32 _totalPassed = 0;
            //    Int32 _totalFailed = 0;
            //    Int32 _totalIgnored = 0;
            //    foreach (TestClasses _class in _project.Classes)
            //    {
            //        TimeSpan durationClass = TimeSpan.Parse("00:00:00.00");
            //        DataRow[] methods = ds.Tables[TABLE_TESTMETHOD].Select(COLUMN_CLASSNAME + " like '" + _class.Name + ", " + _project.Name + ", %'");
            //        if (methods != null && methods.Count() > 0)
            //        {
            //            _class.Methods = new List<TestClassMethods>();
            //            Int32 _passed = 0;
            //            Int32 _failed = 0;
            //            Int32 _ignored = 0;
            //            foreach (DataRow dr in methods)
            //            {
            //                TimeSpan durationMethod = TimeSpan.Parse("00:00:00.00");
            //                TestClassMethods _method = GetTestMethodDetails(ds, dr[ATTRIBUTE_TESTMETHODID].ToString());
            //                switch (_method.Status.ToUpper())
            //                {
            //                    case "PASSED":
            //                        _passed++;
            //                        break;
            //                    case "FAILED":
            //                        _failed++;
            //                        break;
            //                    default:
            //                        _ignored++;
            //                        break;
            //                }

            //                _class.Passed = _passed;
            //                _class.Failed = _failed;
            //                _class.Ignored = _ignored;
            //                _class.Total = (_passed + _failed + _ignored);
            //                _class.Methods.Add(_method);

            //                durationClass += TimeSpan.Parse(_method.Duration);
            //            }
            //        }
            //        _totalPassed += _class.Passed;
            //        _totalFailed += _class.Failed;
            //        _totalIgnored += _class.Ignored;

            //        _class.Duration = durationClass.ToString();
            //        durationProject += TimeSpan.Parse(_class.Duration);
            //    }
            //    _project.Passed = _totalPassed;
            //    _project.Failed = _totalFailed;
            //    _project.Ignored = _totalIgnored;
            //    _project.Total = (_totalPassed + _totalFailed + _totalIgnored);

            //    _project.Duration = durationProject.ToString();
            //    durationProject += TimeSpan.Parse(_project.Duration);
            //}
        }
        private static TestClassMethods GetTestMethodDetails(DataSet ds, string testID)
        {
            TestClassMethods _method = null;
            DataRow[] methods = ds.Tables[TABLE_UNITTESTRESULT].Select(ATTRIBUTE_TESTID + "='" + testID + "'");
            if (methods != null && methods.Count() > 0)
            {
                _method = new TestClassMethods();
                foreach (DataRow dr in methods)
                {
                    _method.Name = dr[COLUMN_TESTNAME].ToString();
                    _method.Status = dr[COLUMN_OUTCOME].ToString();//(Enums.TestStatus)Enum.Parse(typeof(Enums.TestStatus), dr[COLUMN_OUTCOME].ToString());
                    _method.Error = GetErrorInfo(ds, testID);
                    _method.Duration = dr[COLUMN_DURATION].ToString();
                }
            }
            return _method;
        }
        private static ErrorInfo GetErrorInfo(DataSet ds, string testID)
        {
            ErrorInfo _error = null;
            DataRow[] errorMethod = ds.Tables[TAG_ERRORINFO].Select(ATTRIBUTE_TESTMETHODID + "='" + testID + "'");
            if (errorMethod != null && errorMethod.Count() > 0)
            {
                _error = new ErrorInfo();
                string[] delimiters = new string[] { ":line " };
                foreach (DataRow dr in errorMethod)
                {
                    _error.Message = dr[COLUMN_MESSAGE].ToString();
                    _error.StackTrace = dr[COLUMN_STACKTRACE].ToString();

                    string strLineNo="0";
                    try
                    {
                      strLineNo = _error.StackTrace.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)[1];
                    }
                    catch(IndexOutOfRangeException e )
                    {
                            Console.WriteLine("[WRANNING]: there is no stack line number in the TRX(see tag -stacktrace");
                    }
                

                    Int32 LineNo;
                    if (Int32.TryParse(strLineNo, out LineNo))
                    {
                        LineNo = Convert.ToInt32(strLineNo);
                    }
                    else
                    {
                        delimiters = strLineNo.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (delimiters.Length > 0)
                        {
                            if (Int32.TryParse(delimiters[0], out LineNo))
                            {
                                LineNo = Convert.ToInt32(delimiters[0]);
                            }
                        }
                    }
                    _error.LineNo = LineNo;
                }
            }
            return _error;
        }
        private static void CreateTestHierarchy()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("var treeData = '");
            foreach (var _project in testProjects)
            {
                sb.Append("<li><span class=\"testProject\">" + _project.Name + "</span>");
                sb.Append("<ul>");
                foreach (var _class in _project.Classes)
                {
                    //Remove project name from name space if exists.
                    string classname = _class.Name;
                    string projectname = _project.Name + ".";
                    string[] tmp = _class.Name.Split(new string[] { projectname }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length >= 2)
                        classname = (tmp[0] == _project.Name) ? ConvertStringArrayToString(tmp, 1) : tmp[0];
                    else if (tmp.Length == 1)
                        classname = tmp[0];

                    sb.Append("<li><span class=\"testClass\">" + classname + "</span>");
                    sb.Append("<ul>");
                    
                    foreach (var _method in _class.Methods)
                    {
                        string imgStatus = "StatusFailed";
                        switch (_method.Status)
                        {
                            case "Passed":
                                imgStatus = "StatusPassed";
                                break;
                            case "Ignored":
                                imgStatus = "StatusIngnored";
                                break;
                            case "Failed":
                                imgStatus = "StatusFailed";
                                break;
                        }
                        sb.Append("<li><span class=\"testMethod\">" + _method.Name + "<img src=\"Images/" + imgStatus + ".png\" height=\"10\" width=\"10\" /></span></li>");
                    }
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append("</li>");
            }
            sb.Append("';");
            string htmlTestHierarchy = sb.ToString();
            WriteFile("TestHierarchy.js", htmlTestHierarchy);
        }
        private static string ConvertStringArrayToString(string[] array, int startIndex)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = startIndex; i < array.Length; i++)
            {
                builder.Append(array[i]);
            }
            return builder.ToString();
        }
        private static void CreateTestResultTable()
        {
            try
            {
                StringBuilder sbenv = new StringBuilder();
                sbenv.Append("var environment = {");
                sbenv.Append("'TestCodebase':'" + testEnvironmentInfo.TestCodebase + "',");
                sbenv.Append("'Timestamp':'" + testEnvironmentInfo.Timestamp + "',");
                sbenv.Append("'MachineName':'" + testEnvironmentInfo.MachineName + "',");
                sbenv.Append("'UserName':'" + testEnvironmentInfo.UserName + "',");
                sbenv.Append("'OriginalTRXFile':'" + testEnvironmentInfo.OriginalTRXFile + "'");
                sbenv.Append("};");
                WriteFile("Environment.js", sbenv.ToString());

                StringBuilder sb = new StringBuilder();
                sb.Append("$(function () {");
                sb.Append(" $('#dvTestCodebase').text(environment.TestCodebase);");
                sb.Append(" $('#dvGeneratedDate').text(environment.Timestamp);");
                sb.Append(" $('#dvMachineName').text(environment.MachineName);");
                sb.Append(" $('#dvUserName').text(environment.UserName);");
                sb.Append(" $('#dvTRXFileName').text(environment.OriginalTRXFile);");
                sb.Append("var mydata = [");
                int counter = 0;

                foreach (var _project in testProjects)
                {
                    counter++;
                    int total = _project.Passed + _project.Failed + _project.Ignored;
                    double percentPass = (_project.Passed * 100);
                    if (percentPass > 0) percentPass = percentPass / total;
                    double percentFail = (_project.Failed * 100);
                    if (percentFail > 0) percentFail = percentFail / total;
                    double percentIgnore = (_project.Ignored * 100);
                    if (percentIgnore > 0) percentIgnore = percentIgnore / total;
                    string strPercent = string.Format("{0},{1},{2}", percentPass, percentFail, percentIgnore);
                    string strProject = string.Format("{{id: \"{0}\", parent: \"{1}\", level: \"{2}\", Name:  \"{3}\", Passed: \"{4}\", Failed: \"{5}\", Ignored: \"{6}\", Percent: \"{7}\", Progress: \"{8}\", Time: \"{9}\", Message: \"{10}\", StackTrace: \"{11}\", LineNo: \"{12}\", isLeaf: {13}, expanded: {14}, loaded: {15}}},", counter, "", "0", _project.Name, _project.Passed, _project.Failed, _project.Ignored, string.Format("{0:00.00}", percentPass), strPercent, TimeSpan.Parse(_project.Duration).TotalMilliseconds, "", "", "", "false", "true", "true");
                    sb.Append(strProject);
                    int projParent = counter;

                    projectChartDataValue = "var projectData = [" + _project.Passed + ", " + _project.Failed + ", " + _project.Ignored + "];";

                    foreach (var _class in _project.Classes)
                    {
                        counter++;
                        total = _class.Passed + _class.Failed + _class.Ignored;
                        percentPass = (_class.Passed * 100);
                        if (percentPass > 0) percentPass = percentPass / total;
                        percentFail = (_class.Failed * 100);
                        if (percentFail > 0) percentFail = percentFail / total;
                        percentIgnore = (_class.Ignored * 100);
                        if (percentIgnore > 0) percentIgnore = percentIgnore / total;
                        strPercent = string.Format("{0},{1},{2}", percentPass, percentFail, percentIgnore);

                        //Remove project name from name space if exists.
                        string classname = _class.Name;
                        string projectname = _project.Name + ".";
                        string[] tmp = _class.Name.Split(new string[] { projectname }, StringSplitOptions.RemoveEmptyEntries);
                        if (tmp.Length >= 2)
                            classname = (tmp[0] == _project.Name) ? ConvertStringArrayToString(tmp, 1) : tmp[0];
                        else if (tmp.Length == 1)
                            classname = tmp[0];

                        string strClass = string.Format("{{id: \"{0}\", parent: \"{1}\", level: \"{2}\", Name:  \"{3}\", Passed: \"{4}\", Failed: \"{5}\", Ignored: \"{6}\", Percent: \"{7}\", Progress: \"{8}\", Time: \"{9}\", Message: \"{10}\", StackTrace: \"{11}\", LineNo: \"{12}\", isLeaf: {13}, expanded: {14}, loaded: {15}}},", counter, projParent, "1", classname, _class.Passed, _class.Failed, _class.Ignored, string.Format("{0:00.00}", percentPass), strPercent, TimeSpan.Parse(_class.Duration).TotalMilliseconds, "", "", "", "false", "true", "true");
                        sb.Append(strClass);
                        int classParent = counter;

                        classChartDataValue += "[" + _class.Passed + ", " + _class.Failed + ", " + _class.Ignored + "],";
                        classChartDataText += "'" + classname + "',";

                        foreach (var _method in _class.Methods)
                        {
                            counter++;
                            int _passed = 0;
                            int _failed = 0;
                            int _ignored = 0;
                            percentPass = 0.0;
                            strPercent = "";

                            methoChartDataValue += TimeSpan.Parse(_method.Duration).TotalMilliseconds + ",";
                            methoChartDataText += "'" + _method.Name + "',";

                            switch (_method.Status)
                            {
                                case "Passed":
                                    _passed = 1;
                                    percentPass = 100;
                                    strPercent = "100,0,0";
                                    methoChartDataColor += "testResultColor[0],";
                                    break;
                                case "Failed":
                                    _failed = 1;
                                    strPercent = "0,100,0";
                                    methoChartDataColor += "testResultColor[1],";
                                    break;
                                case "Ignored":
                                    _ignored = 1;
                                    strPercent = "0,0,100";
                                    methoChartDataColor += "testResultColor[2],";
                                    break;
                            }

                            string strError = "";
                            string strStack = "";
                            string strLine = "";
                            if (_method.Error != null)
                            {
                                strError = _method.Error.Message.Replace("\r\n", "").Replace("\"", "'");
                                strStack = _method.Error.StackTrace.Replace("\r\n", "").Replace("\"", "'");
                                strLine = _method.Error.LineNo.ToString();
                            }

                            string strMethod = string.Format("{{id: \"{0}\", parent: \"{1}\", level: \"{2}\", Name:  \"{3}\", Passed: \"{4}\", Failed: \"{5}\", Ignored: \"{6}\", Percent: \"{7}\", Progress: \"{8}\", Time: \"{9}\", Message: \"{10}\", StackTrace: \"{11}\", LineNo: \"{12}\", isLeaf: {13}, expanded: {14}, loaded: {15}}},", counter, classParent, "2", _method.Name, _passed, _failed, _ignored, string.Format("{0:00.00}", percentPass), strPercent, TimeSpan.Parse(_method.Duration).TotalMilliseconds, strError, strStack, strLine, "true", "false", "true");
                            sb.Append(strMethod);
                        }
                    }

                    classChartDataValue = "var classDataValue = [" + classChartDataValue + "];";
                    classChartDataText = "var classDataText = [" + classChartDataText + "];";

                    methoChartDataValue = "var methoDataValue = [" + methoChartDataValue + "];";
                    methoChartDataText = "var methoDataText = [" + methoChartDataText + "];";
                    methoChartDataColor = "var methoDataColor = [" + methoChartDataColor + "];";
                }
                sb.Append("],");
                sb.Append("getColumnIndexByName = function (grid, columnName) {");
                sb.Append("var cm = grid.jqGrid('getGridParam', 'colModel');");
                sb.Append("for (var i = 0; i < cm.length; i += 1) {");
                sb.Append("if (cm[i].name === columnName) {");
                sb.Append("return i;");
                sb.Append("}");
                sb.Append("}");
                sb.Append("return -1;");
                sb.Append("},");
                sb.Append("grid = $('#treegrid');");
                sb.Append("grid.jqGrid({");
                sb.Append("datatype: 'jsonstring',");
                sb.Append("datastr: mydata,");
                sb.Append("colNames: ['Id', 'Name', 'Passed', 'Failed', 'Ignored', '%', '', 'Time', 'Message','StackTrace','LineNo'],");
                sb.Append("colModel: [");
                sb.Append("{ name: 'id', index: 'id', width: 1, hidden: true, key: true },");
                sb.Append("{ name: 'Name', index: 'Name', width: 380 },");
                sb.Append("{ name: 'Passed', index: 'Passed', width: 70, align: 'right', formatter: testCounterFormat },");
                sb.Append("{ name: 'Failed', index: 'Failed', width: 70, align: 'right', formatter: testCounterFormat },");
                sb.Append("{ name: 'Ignored', index: 'Ignored', width: 70, align: 'right', formatter: testCounterFormat },");
                sb.Append("{ name: 'Percent', index: 'Percent', width: 50, align: 'right' },");
                sb.Append("{ name: 'Progress', index: 'Progress', width: 200, align: 'right', formatter: progressFormat },");
                sb.Append("{ name: 'Time', index: 'Time', width: 75, align: 'right'},");
                sb.Append("{ name: 'Message', index: 'Message', hidden: true, width: 100, align: 'right'},");
                sb.Append("{ name: 'StackTrace', index: 'StackTrace', hidden: true, width: 100, align: 'right'},");
                sb.Append("{ name: 'LineNo', index: 'LineNo', width: 100, hidden: true, align: 'right'}],");
                sb.Append("height: 'auto',");
                sb.Append("gridview: true,");
                sb.Append("rowNum: 10000,");
                sb.Append("sortname: 'id',");
                sb.Append("treeGrid: true,");
                sb.Append("treeGridModel: 'adjacency',");
                sb.Append("treedatatype: 'local',");
                sb.Append("ExpandColumn: 'Name',");

                sb.Append("ondblClickRow: function(id) {");
                sb.Append("parent.innerLayout.open('south');");
                sb.Append("setErrorInfo(id);");
                sb.Append("},");

                sb.Append("onSelectRow: function(id){");
                sb.Append("setErrorInfo(id);");
                sb.Append("},");

                sb.Append("jsonReader: {");
                sb.Append("repeatitems: false,");
                sb.Append("root: function (obj) { return obj; },");
                sb.Append("page: function (obj) { return 1; },");
                sb.Append("total: function (obj) { return 1; },");
                sb.Append("records: function (obj) { return obj.length; }");
                sb.Append("}");
                sb.Append("});");

                sb.Append("function setErrorInfo(id) {");
                sb.Append("var doc = $('#tblError', top.document);");
                sb.Append("doc.find('#dvErrorMessage').text($('#treegrid').getRowData(id)['Message']);");
                sb.Append("doc.find('#dvLineNumber').text($('#treegrid').getRowData(id)['LineNo']);");
                sb.Append("doc.find('#dvStackTrace').text($('#treegrid').getRowData(id)['StackTrace']);");
                sb.Append("}");

                sb.Append("function progressFormat(cellvalue, options, rowObject) {");
                sb.Append("var progress = cellvalue.split(',');");
                sb.Append("var pass = Math.round(progress[0]) * 2;");
                sb.Append("var fail = Math.round(progress[1]) * 2;");
                sb.Append("var ignore = Math.round(progress[2]) * 2;");
                sb.Append("var strProgress = \"<div class='ProgressWrapper'>");
                sb.Append("<div class='ProgressPass' title='\"+ Number(progress[0]).toFixed(2) +\"% Passed' style='width: \" + pass + \"px'></div>");
                sb.Append("<div class='ProgressFail' title='\"+ Number(progress[1]).toFixed(2) +\"% Failed' style='width: \" + fail + \"px'></div>");
                sb.Append("<div class='ProgressIgnore' title='\"+ Number(progress[2]).toFixed(2) +\"% Ignored' style='width: \" + ignore + \"px'></div>");
                sb.Append("</div>\";");
                sb.Append("return strProgress;");
                sb.Append("}");

                sb.Append("function testCounterFormat(cellvalue, options, rowObject) {");
                sb.Append("return cellvalue;");
                sb.Append("}");
                sb.Append("grid.jqGrid('setLabel', 'Passed', '', { 'text-align': 'right' });");
                sb.Append("grid.jqGrid('setLabel', 'Failed', '', { 'text-align': 'right' });");
                sb.Append("grid.jqGrid('setLabel', 'Ignored', '', { 'text-align': 'right' });");
                sb.Append("grid.jqGrid('setLabel', 'Percent', '', { 'text-align': 'right' });");
                sb.Append("});");
                string xmlTestResultTable = sb.ToString().Replace("},],", "}],");
                WriteFile("Table.js", xmlTestResultTable);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        private static void CreateTestResultChart()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("var testResultStatus = ['Passed', 'Failed', 'Ignored'];");
            sb.Append("var testResultColor = ['#ABD874', '#E18D87', '#F4AD7C'];");
            sb.Append(projectChartDataValue);
            sb.Append(classChartDataValue);
            sb.Append(classChartDataText);
            sb.Append(methoChartDataValue);
            sb.Append(methoChartDataText);
            sb.Append(methoChartDataColor);

            string xmlTestResultTable = sb.ToString().Replace(",]", "]");
            WriteFile("Chart.js", xmlTestResultTable);
        }
        private static void WriteFile(string FileName, string FileContent)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(folderPath, FileName)))
            {
                file.WriteLine(FileContent);
            }
        }
        private static void GenerateTRXFile(string MsTestExePath, string TestContainerFilePath)
        {
            trxFilePath = Path.Combine(folderPath, FILENAME_TRX);
            string commandText = "\"" + MsTestExePath + "\" /testcontainer:\"" + TestContainerFilePath + "\" /resultsfile:\"" + trxFilePath + "\"";
            WriteFile("mstestrunner.bat", commandText);
            ExecuteBatchFile();
        }
        private static void ExecuteBatchFile()
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = Path.Combine(folderPath, "mstestrunner.bat");
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }
        private static bool RecognizeParameters(string[] args)
        {
            if (args.Length >= 4 && args.Length <= 10)
            {
                int i = 0;
                while (i < args.Length)
                {
                    switch (args[i].ToLower())
                    {
                        case "/m":
                        case "/mstestexepath":
                        case "-m":
                        case "-mstestexepath":
                            if (args.Length > i)
                            {
                                MSTestExePathParam = args[i + 1];
                                i += 2;
                            }
                            else
                                return false;
                            break;
                        case "/tc":
                        case "/testcontainerfolderpath":
                        case "-tc":
                        case "-testcontainerfolderpath":
                            if (args.Length > i)
                            {
                                TestContainerFolderPathParam = args[i + 1];
                                i += 2;
                            }
                            else
                                return false;
                            break;
                        case "/t":
                        case "/trxfilepath":
                        case "-t":
                        case "-trxfilepath":
                            if (args.Length > i)
                            {
                                trxFilePath = args[i + 1];
                                i += 2;
                            }
                            else
                                return false;
                            break;
                        case "/d":
                        case "/destinationfolderpath":
                        case "-d":
                        case "-destinationfolderpath":
                            if (args.Length > i)
                            {
                                DestinationFolderParam = args[i + 1];
                                i += 2;
                            }
                            else
                                return false;
                            break;
                        /*case "/l":
                        case "/logfile":
                        case "-l":
                        case "-logfile":
                            if (args.Length > i)
                            {
                                LogFileParam = args[i + 1];
                                i += 2;
                            }
                            else
                                return false;
                            break;
                        case "/h":
                        case "/helpfile":
                        case "-h":
                        case "-helpfile":
                            if (args.Length > i)
                            {
                                HelpFileParam = args[i + 1];
                                i += 2;
                            }
                            else
                                return false;
                            break;*/
                        case "/?":
                        case "/help":
                        case "-?":
                        case "-help":
                            return false;
                        default:
                            Console.WriteLine("Error: Unrecognized parameter\n");
                            return false;
                    }
                }
                return true;
            }
            return false;
        }
        private static void DisplayCommandLineHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0}.exe\n", Title));
            sb.Append(string.Format("[</M MSTestExePath>\n"));
            sb.Append(string.Format(" </TC TestContainerFolderPath>]    :optional if you set /T TRXFilePath\n"));
            sb.Append(string.Format("[</T TRXFilePath>]\n"));
            sb.Append(string.Format("</D DestinationFolder>             \n"));
            sb.Append(string.Format("</? Help>\n"));
            Console.Write(sb.ToString());
            Console.ReadKey();
        }
        private static bool CopyFilesWithSubFolders(string SourcePath, string DestinationPath, bool overwriteexisting, string Pattern = "*.*")
        {
            bool ret = true;
            try
            {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false) Directory.CreateDirectory(DestinationPath);
                    foreach (string fls in Directory.GetFiles(SourcePath, Pattern))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyFilesWithSubFolders(drs, DestinationPath + drinfo.Name, overwriteexisting, Pattern) == false) ret = false;
                    }
                }
                else
                {
                    ret = false;
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }
        #endregion
    }
}
