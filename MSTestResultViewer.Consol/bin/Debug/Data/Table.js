$(function () { $('#dvTestCodebase').text(environment.TestCodebase); $('#dvGeneratedDate').text(environment.Timestamp); $('#dvMachineName').text(environment.MachineName); $('#dvUserName').text(environment.UserName); $('#dvTRXFileName').text(environment.OriginalTRXFile);var mydata = [{id: "1", parent: "", level: "0", Name:  "VisionmapAutomation", Passed: "0", Failed: "2", Ignored: "0", Percent: "00.00", Progress: "0,100,0", Time: "432.3069", Message: "", StackTrace: "", LineNo: "", isLeaf: false, expanded: true, loaded: true},{id: "2", parent: "1", level: "1", Name:  "CodedUITest1", Passed: "0", Failed: "2", Ignored: "0", Percent: "00.00", Progress: "0,100,0", Time: "432.3069", Message: "", StackTrace: "", LineNo: "", isLeaf: false, expanded: true, loaded: true},{id: "3", parent: "2", level: "2", Name:  "Games", Passed: "0", Failed: "1", Ignored: "0", Percent: "00.00", Progress: "0,100,0", Time: "372.4309", Message: "Test method VisionmapAutomation.CodedUITest1.Games threw exception: System.IO.FileNotFoundException: Could not load file or assembly 'AutomationInfra, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies. The system cannot find the file specified.WRN: Assembly binding logging is turned OFF.To enable assembly bind failure logging, set the registry value [HKLM\Software\Microsoft\Fusion!EnableLog] (DWORD) to 1.Note: There is some performance penalty associated with assembly bind failure logging.To turn this feature off, remove the registry value [HKLM\Software\Microsoft\Fusion!EnableLog].", StackTrace: "    at VisionmapAutomation.CodedUITest1.Games()", LineNo: "0", isLeaf: true, expanded: false, loaded: true},{id: "4", parent: "2", level: "2", Name:  "TestedFuncs", Passed: "0", Failed: "1", Ignored: "0", Percent: "00.00", Progress: "0,100,0", Time: "59.876", Message: "Test method VisionmapAutomation.CodedUITest1.TestedFuncs threw exception: System.IO.FileNotFoundException: Could not load file or assembly 'AutomationInfra, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies. The system cannot find the file specified.WRN: Assembly binding logging is turned OFF.To enable assembly bind failure logging, set the registry value [HKLM\Software\Microsoft\Fusion!EnableLog] (DWORD) to 1.Note: There is some performance penalty associated with assembly bind failure logging.To turn this feature off, remove the registry value [HKLM\Software\Microsoft\Fusion!EnableLog].", StackTrace: "    at VisionmapAutomation.CodedUITest1.TestedFuncs()", LineNo: "0", isLeaf: true, expanded: false, loaded: true}],getColumnIndexByName = function (grid, columnName) {var cm = grid.jqGrid('getGridParam', 'colModel');for (var i = 0; i < cm.length; i += 1) {if (cm[i].name === columnName) {return i;}}return -1;},grid = $('#treegrid');grid.jqGrid({datatype: 'jsonstring',datastr: mydata,colNames: ['Id', 'Name', 'Passed', 'Failed', 'Ignored', '%', '', 'Time', 'Message','StackTrace','LineNo'],colModel: [{ name: 'id', index: 'id', width: 1, hidden: true, key: true },{ name: 'Name', index: 'Name', width: 380 },{ name: 'Passed', index: 'Passed', width: 70, align: 'right', formatter: testCounterFormat },{ name: 'Failed', index: 'Failed', width: 70, align: 'right', formatter: testCounterFormat },{ name: 'Ignored', index: 'Ignored', width: 70, align: 'right', formatter: testCounterFormat },{ name: 'Percent', index: 'Percent', width: 50, align: 'right' },{ name: 'Progress', index: 'Progress', width: 200, align: 'right', formatter: progressFormat },{ name: 'Time', index: 'Time', width: 75, align: 'right'},{ name: 'Message', index: 'Message', hidden: true, width: 100, align: 'right'},{ name: 'StackTrace', index: 'StackTrace', hidden: true, width: 100, align: 'right'},{ name: 'LineNo', index: 'LineNo', width: 100, hidden: true, align: 'right'}],height: 'auto',gridview: true,rowNum: 10000,sortname: 'id',treeGrid: true,treeGridModel: 'adjacency',treedatatype: 'local',ExpandColumn: 'Name',ondblClickRow: function(id) {parent.innerLayout.open('south');setErrorInfo(id);},onSelectRow: function(id){setErrorInfo(id);},jsonReader: {repeatitems: false,root: function (obj) { return obj; },page: function (obj) { return 1; },total: function (obj) { return 1; },records: function (obj) { return obj.length; }}});function setErrorInfo(id) {var doc = $('#tblError', top.document);doc.find('#dvErrorMessage').text($('#treegrid').getRowData(id)['Message']);doc.find('#dvLineNumber').text($('#treegrid').getRowData(id)['LineNo']);doc.find('#dvStackTrace').text($('#treegrid').getRowData(id)['StackTrace']);}function progressFormat(cellvalue, options, rowObject) {var progress = cellvalue.split(',');var pass = Math.round(progress[0]) * 2;var fail = Math.round(progress[1]) * 2;var ignore = Math.round(progress[2]) * 2;var strProgress = "<div class='ProgressWrapper'><div class='ProgressPass' title='"+ Number(progress[0]).toFixed(2) +"% Passed' style='width: " + pass + "px'></div><div class='ProgressFail' title='"+ Number(progress[1]).toFixed(2) +"% Failed' style='width: " + fail + "px'></div><div class='ProgressIgnore' title='"+ Number(progress[2]).toFixed(2) +"% Ignored' style='width: " + ignore + "px'></div></div>";return strProgress;}function testCounterFormat(cellvalue, options, rowObject) {return cellvalue;}grid.jqGrid('setLabel', 'Passed', '', { 'text-align': 'right' });grid.jqGrid('setLabel', 'Failed', '', { 'text-align': 'right' });grid.jqGrid('setLabel', 'Ignored', '', { 'text-align': 'right' });grid.jqGrid('setLabel', 'Percent', '', { 'text-align': 'right' });});