using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Json;

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
        static string tempFolder ="";
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
                    string tempPath = Environment.GetEnvironmentVariable("temp");
                    long ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
                    ticks /= 10000000; //Convert windows ticks to seconds
                    var timestamp = ticks.ToString();
                    tempFolder = Path.Combine(tempPath, timestamp,"Data");
                    Directory.CreateDirectory(tempFolder);
                   
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
                    CopyFilesWithSubFolders(Path.Combine(appPath, "web","Images"), Path.Combine(DestinationFolderParam, "Images"), true);
                    CopyFilesWithSubFolders(Path.Combine(appPath, "web", "Javascripts"), Path.Combine(DestinationFolderParam, "Javascripts"), true);
                    CopyFilesWithSubFolders(Path.Combine(appPath, "web", "Library"), Path.Combine(DestinationFolderParam, "Library"), true);
                    CopyFilesWithSubFolders(Path.Combine(appPath, "web", "RGraph"), Path.Combine(DestinationFolderParam, "RGraph"), true);
                    CopyFilesWithSubFolders(Path.Combine(appPath, "web", "Styles"), Path.Combine(DestinationFolderParam, "Styles"), true);
                    CopyFilesWithSubFolders(tempFolder, Path.Combine(DestinationFolderParam, "Data"), true);
                    CopyFilesWithSubFolders(Path.Combine(appPath,"web", "Pages"), DestinationFolderParam, true);
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

            Console.WriteLine(string.Format("Start gathering test environment information...\n"));
            System.Threading.Thread.Sleep(SLEEP_EXECUTION_TIME);
            SetTestEnvironmentInfo();

            Console.WriteLine(string.Format("Start gathering test result summary...\n"));
            System.Threading.Thread.Sleep(SLEEP_EXECUTION_TIME);
            SetTestResultSummary();

            Console.WriteLine(string.Format("Start gathering test classes methods information...\n"));
            System.Threading.Thread.Sleep(SLEEP_EXECUTION_TIME);
            SetTestClassMethods();

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
        private static void SetTestEnvironmentInfo()
        {
            //TODO: add validation for size >0
            var codebase = testRun.TestDefinitions[0].TestMethod[0].codeBase;
            var runUser = testRun.runUser;
            var trxfile = testRun.name;
            var cre = testRun.Times[0].creation;
            var creationDate = Convert.ToDateTime(cre);
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
        private static void SetTestResultSummary()
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

        private static TestRunResultsUnitTestResult GetResultOfTest(TestRunTestDefinitionsUnitTest test)
        {
            TestRunResultsUnitTestResult res = null;
            try
            {
                if (null != test)
                {
                    var id = test.id;
                    foreach (var testRes in testRun.Results)
                    {
                        if (testRes.testId.Equals(id))
                        {
                            res = testRes;
                            break;
                        }
                    }
                }


            }
            catch (Exception e)
            {

            }


            return res;



        }

        private static void SetTestClassMethods()
        {
            HashSet<string> projects = new HashSet<string>();
            var comma = ", ";
            testProjects = new List<TestProjects>();
            var testDef = testRun.TestDefinitions;

            foreach (var unittest in testDef)
            {
                bool passed = false;
                bool failed = false;
                bool ignore = false;
                var testRes = GetResultOfTest(unittest);

                var classFullDetails = unittest.TestMethod[0].className; //Should be only one TestMethod under UnitTest tag
                int start = classFullDetails.IndexOf(comma);
                var className = classFullDetails.Substring(0, start);
                var temp = classFullDetails.Substring(start + 1);
                int end = temp.IndexOf(comma) - 1;



                var tcm = new TestClassMethods
                {
                    Name = unittest.name,
                    Id = unittest.id,
                    Category = unittest.TestCategory[0].TestCategory,
                    Duration = TimeSpan.Parse(testRes.duration).ToString(),
                    Status = testRes.outcome,
                    Output = testRes.Output[0].StdOut,
                    Type = testRes.testType,
                    Error = GetErrorInfo(unittest)
                };


                var outcome = testRes.outcome;
                switch (outcome.ToUpper())
                {
                    case "PASSED":
                        passed = true;
                        break;
                    case "FAILED":
                        failed = true;
                        break;
                    default:
                        ignore = true;
                        break;
                }



                var projectName = classFullDetails.Substring(start + comma.Length, end).Trim();
                if (projects.Contains(projectName))
                {

                    var p = new TestProjects() { Name = projectName };
                    var projectFound = testProjects.Find(proj => proj.Name.Equals(p.Name));
                    var calssFound = projectFound.Classes.Find(clazz => clazz.Name.Equals(className));

                    if (calssFound == null)
                    {
                        var tc = new TestClasses() { Name = className };

                        if (passed)
                        {
                            tc.Passed++;
                        }
                        else if (failed)
                        {
                            tc.Failed++;
                        }
                        else
                        {
                            tc.Ignored++;
                        }

                        tc.Duration += TimeSpan.Parse(testRes.duration);


                        tc.Methods.Add(tcm);
                        projectFound.Classes.Add(tc);
                    }
                    else
                    {
                        if (passed)
                        {
                            calssFound.Passed++;
                        }
                        else if (failed)
                        {
                            calssFound.Failed++;
                        }
                        else
                        {
                            calssFound.Ignored++;
                        }
                        calssFound.Duration += TimeSpan.Parse(testRes.duration);
                        calssFound.Methods.Add(tcm);
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

                    var tc = new TestClasses { Name = className };

                    if (passed)
                    {
                        tc.Passed++;
                    }
                    else if (failed)
                    {
                        tc.Failed++;
                    }
                    else
                    {
                        tc.Ignored++;
                    }
                    tc.Duration += TimeSpan.Parse(testRes.duration);

                    tc.Methods.Add(tcm);
                    testproject.Classes.Add(tc);
                    testProjects.Add(testproject);

                }

            }

            //TODO: support more than one test project[each project should have totla pass,total failed,total ignore ,and total tests,and total time of all the tests]
            foreach (var testP in testProjects)
            {
                Int32 passed = 0;
                Int32 failed = 0;
                Int32 ignored = 0;
                TimeSpan projectTotalDuration = TimeSpan.Parse("00:00:00.00");

                foreach (var testReuslt in testRun.Results)
                {
                    var duration = testReuslt.duration;
                    var testOutcome = testReuslt.outcome;

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
                    projectTotalDuration += TimeSpan.Parse(duration);


                }
                testP.Duration = projectTotalDuration.ToString();
                testP.Passed = passed;
                testP.Failed = failed;
                testP.Ignored = ignored;


                testP.Total = (passed + failed + ignored);
            }

        
        }
      
        private static ErrorInfo GetErrorInfo(TestRunTestDefinitionsUnitTest test)
        {

            var result = GetResultOfTest(test);
            ErrorInfo _error = null;

            if (result != null && result.outcome.ToUpper().Equals("FAILED"))
            {
                _error = new ErrorInfo();
                string[] delimiters = new string[] { ":line " };
               
                _error.Message = result.Output[0].ErrorInfo[0].Message;
                _error.StackTrace = result.Output[0].ErrorInfo[0].StackTrace.ToString(CultureInfo.InvariantCulture);

               

                string strLineNo = "0";
                try
                {
                    strLineNo = _error.StackTrace.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)[1];
                }
                catch (IndexOutOfRangeException e)
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
                //}
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
           List<TableItemInfo> tableItems = new List<TableItemInfo>();
            try
            {
                StringBuilder sbenv = new StringBuilder();
                sbenv.Append("var environment = {");
                sbenv.Append("'TestCodebase':'" + testEnvironmentInfo.TestCodebase + "',");
                sbenv.Append("'Timestamp':'" + testEnvironmentInfo.Timestamp + "',");
                sbenv.Append("'MachineName':'" + testEnvironmentInfo.MachineName + "',");
                sbenv.Append("'UserName':'" + testEnvironmentInfo.UserName + "',");
                sbenv.Append("'OriginalTRXFile':'" + testEnvironmentInfo.OriginalTRXFile + "'");
                sbenv.Append("}; ");
                WriteFile("Environment.js", sbenv.ToString());

                StringBuilder sb = new StringBuilder();
                sb.Append("$(function () { \n \n ");
                sb.Append(" $('#dvTestCodebase').text(environment.TestCodebase);\n ");
                sb.Append(" $('#dvGeneratedDate').text(environment.Timestamp);\n");
                sb.Append(" $('#dvMachineName').text(environment.MachineName);\n");
                sb.Append(" $('#dvUserName').text(environment.UserName);\n");
                sb.Append(" $('#dvTRXFileName').text(environment.OriginalTRXFile);\n\n");
                sb.Append("var mydata =");
                int counter = 0;

                foreach (var _project in testProjects)
                {
                    counter++;
                    int total = _project.Total;
                    double percentPass = (_project.Passed * 100);
                    if (percentPass > 0) percentPass = percentPass / total;
                    double percentFail = (_project.Failed * 100);
                    if (percentFail > 0) percentFail = percentFail / total;
                    double percentIgnore = (_project.Ignored * 100);
                    if (percentIgnore > 0) percentIgnore = percentIgnore / total;
                    string strPercent = string.Format("{0},{1},{2}", percentPass, percentFail, percentIgnore);

                    var pTime =TimeSpan.Parse(_project.Duration).TotalMilliseconds;
                    TableItemInfo root = new TableItemInfo(){id=counter,parent=0,level = "0",Name=_project.Name,Passed =_project.Passed,Failed =_project.Failed,Ignored = _project.Ignored,Percent = string.Format("{0:00.00}", percentPass),Progress =strPercent,Time =pTime,Message = "none",StackTrace = "none",LineNo = "none",isLeaf = false,expanded = true,loaded = true};

                    tableItems.Add(root);

                  
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


                        var cTime = _class.Duration.TotalMilliseconds;
                        TableItemInfo classInfo = new TableItemInfo() 
                        {   id = counter,
                            parent = projParent, 
                            level = "1",
                            Name = classname,
                            Passed = _class.Passed,
                            Failed = _class.Failed,
                            Ignored = _class.Ignored, 
                            Percent = string.Format("{0:00.00}",percentPass),
                            Progress = strPercent,
                            Time = cTime,
                            Message = "none", 
                            StackTrace = "none",
                            LineNo = "none", 
                            isLeaf = false, 
                            expanded = true, 
                            loaded = true 
                        };

                        tableItems.Add(classInfo);
                      
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

                            

                            var mTime = _class.Duration.TotalMilliseconds;
                            TableItemInfo methodInfo = new TableItemInfo()
                            {
                                id = counter,
                                parent = classParent,
                                level = "2",
                                Name = _method.Name,
                                Passed = _passed,
                                Failed = _failed,
                                Ignored = _ignored,
                                Percent = string.Format("{0:00.00}", percentPass),
                                Progress = strPercent,
                                Time = mTime,
                                Message = strError,
                                StackTrace = strStack,
                                LineNo = strLine,
                                isLeaf = true,
                                expanded = false,
                                loaded = true,
                                TestOutput = _method.Output,
                            };

                            tableItems.Add(methodInfo);
                        }
                    }

                    classChartDataValue = "var classDataValue = [" + classChartDataValue + "];";
                    classChartDataText = "var classDataText = [" + classChartDataText + "];";

                    methoChartDataValue = "var methoDataValue = [" + methoChartDataValue + "];";
                    methoChartDataText = "var methoDataText = [" + methoChartDataText + "];";
                    methoChartDataColor = "var methoDataColor = [" + methoChartDataColor + "];";
                }

             
                sb.Append(tableItems.ToJSON());

                sb.Append("\n\n");
                
                sb.Append("getColumnIndexByName = function (grid, columnName) {\n");
                sb.Append("var cm = grid.jqGrid('getGridParam', 'colModel');\n");
                sb.Append("for (var i = 0; i < cm.length; i += 1) {\n");
                sb.Append("if (cm[i].name === columnName) {\n");
                sb.Append("return i;\n");
                sb.Append("}\n");
                sb.Append("}\n");
                sb.Append("return -1;\n");
                sb.Append("},\n");
                sb.Append("grid = $('#treegrid');\n");
                sb.Append("grid.jqGrid({\n");
                sb.Append("datatype: 'jsonstring',\n");
                sb.Append("datastr: mydata,\n");
                sb.Append("colNames: ['Id', 'Name', 'Passed', 'Failed', 'Ignored', '%', '', 'Time', 'Message','StackTrace','LineNo','TestOutput'],\n");
                sb.Append("colModel: [\n");
                sb.Append("{ name: 'id', index: 'id', width: 1, hidden: true, key: true },\n");
                sb.Append("{ name: 'Name', index: 'Name', width: 380 },\n");
                sb.Append("{ name: 'Passed', index: 'Passed', width: 70, align: 'right', formatter: testCounterFormat },\n");
                sb.Append("{ name: 'Failed', index: 'Failed', width: 70, align: 'right', formatter: testCounterFormat },\n");
                sb.Append("{ name: 'Ignored', index: 'Ignored', width: 70, align: 'right', formatter: testCounterFormat },\n");
                sb.Append("{ name: 'Percent', index: 'Percent', width: 50, align: 'right' },\n");
                sb.Append("{ name: 'Progress', index: 'Progress', width: 250, align: 'right', formatter: progressFormat },\n");
                sb.Append("{ name: 'Time', index: 'Time', width: 100, align: 'right'},\n");
                sb.Append("{ name: 'Message', index: 'Message', hidden: true, width: 100, align: 'right'},\n");
                sb.Append("{ name: 'StackTrace', index: 'StackTrace', hidden: true, width: 100, align: 'right'},\n");
                sb.Append("{ name: 'LineNo', index: 'LineNo', width: 100, hidden: true, align: 'right'},\n");
                sb.Append("{ name: 'TestOutput', index: 'TestOutput', width: 100, hidden: true, align: 'right' }],\n");

              

                sb.Append("height: 'auto',\n");
                sb.Append("gridview: true,\n");
                sb.Append("rowNum: 10000,\n");
                sb.Append("sortname: 'id',\n");
                sb.Append("treeGrid: true,\n");
                sb.Append("treeGridModel: 'adjacency',\n");
                sb.Append("treedatatype: 'local',\n");
                sb.Append("ExpandColumn: 'Name',\n");

                sb.Append("ondblClickRow: function(id) {\n");
                sb.Append("parent.innerLayout.open('south');\n");
                sb.Append("setErrorInfo(id);\n");
                sb.Append("},\n");

                sb.Append("onSelectRow: function(id){\n");
                sb.Append("setErrorInfo(id);\n");
                sb.Append("},\n");

                sb.Append("jsonReader: {\n");
                sb.Append("repeatitems: false,\n");
                sb.Append("root: function (obj) { return obj; },\n");
                sb.Append("page: function (obj) { return 1; },\n");
                sb.Append("total: function (obj) { return 1; },\n");
                sb.Append("records: function (obj) { return obj.length; }\n");
                sb.Append("}\n");
                sb.Append("});\n");

                sb.Append("function setErrorInfo(id) {\n");
                sb.Append("var doc = $('#tblError', top.document);\n");
                sb.Append("doc.find('#dvErrorMessage').text($('#treegrid').getRowData(id)['Message']);\n");
                sb.Append("doc.find('#dvLineNumber').text($('#treegrid').getRowData(id)['LineNo']);\n");
                sb.Append("doc.find('#dvStackTrace').text($('#treegrid').getRowData(id)['StackTrace']);\n");

                //set test log data
                sb.Append("doc.find('#dvTestLog').text($('#treegrid').getRowData(id)['TestOutput']);\n");

                
                
                sb.Append("}\n");

                sb.Append("function progressFormat(cellvalue, options, rowObject) {\n");
                sb.Append("var progress = cellvalue.split(',');\n");
                sb.Append("var pass = Math.round(progress[0]) * 2;\n");
                sb.Append("var fail = Math.round(progress[1]) * 2;\n");
                sb.Append("var ignore = Math.round(progress[2]) * 2;\n");
                sb.Append("var strProgress = \"<div class='ProgressWrapper'>");
                sb.Append("<div class='ProgressPass' title='\"+ Number(progress[0]).toFixed(2) +\"% Passed' style='width: \" + pass + \"px'></div>");
                sb.Append("<div class='ProgressFail' title='\"+ Number(progress[1]).toFixed(2) +\"% Failed' style='width: \" + fail + \"px'></div>");
                sb.Append("<div class='ProgressIgnore' title='\"+ Number(progress[2]).toFixed(2) +\"% Ignored' style='width: \" + ignore + \"px'></div>");
                sb.Append("</div>\";\n");
                sb.Append("return strProgress;\n");
                sb.Append("}\n");

                sb.Append("function testCounterFormat(cellvalue, options, rowObject) {\n");
                sb.Append("return cellvalue;\n");
                sb.Append("}\n");
                sb.Append("grid.jqGrid('setLabel', 'Passed', '', { 'text-align': 'right' });\n");
                sb.Append("grid.jqGrid('setLabel', 'Failed', '', { 'text-align': 'right' });\n");
                sb.Append("grid.jqGrid('setLabel', 'Ignored', '', { 'text-align': 'right' });\n");
                sb.Append("grid.jqGrid('setLabel', 'Percent', '', { 'text-align': 'right' });\n");
                sb.Append("})\n");
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
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(tempFolder, FileName)))
            {
                Console.WriteLine("[Write file - " + Path.Combine(tempFolder, FileName)+"]");
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
