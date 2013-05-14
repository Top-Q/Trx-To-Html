﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5466
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010", IsNullable=false)]
public partial class TestRun {
    
    private TestRunTestSettings[] testSettingsField;
    
    private TestRunTimes[] timesField;
    
    private TestRunResultSummary[] resultSummaryField;
    
    private TestRunTestDefinitionsUnitTest[] testDefinitionsField;
    
    private TestRunTestListsTestList[] testListsField;
    
    private TestRunTestEntriesTestEntry[] testEntriesField;
    
    private TestRunResultsUnitTestResult[] resultsField;
    
    private string idField;
    
    private string nameField;
    
    private string runUserField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("TestSettings")]
    public TestRunTestSettings[] TestSettings {
        get {
            return this.testSettingsField;
        }
        set {
            this.testSettingsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Times")]
    public TestRunTimes[] Times {
        get {
            return this.timesField;
        }
        set {
            this.timesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ResultSummary")]
    public TestRunResultSummary[] ResultSummary {
        get {
            return this.resultSummaryField;
        }
        set {
            this.resultSummaryField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("UnitTest", typeof(TestRunTestDefinitionsUnitTest), IsNullable=false)]
    public TestRunTestDefinitionsUnitTest[] TestDefinitions {
        get {
            return this.testDefinitionsField;
        }
        set {
            this.testDefinitionsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("TestList", typeof(TestRunTestListsTestList), IsNullable=false)]
    public TestRunTestListsTestList[] TestLists {
        get {
            return this.testListsField;
        }
        set {
            this.testListsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("TestEntry", typeof(TestRunTestEntriesTestEntry), IsNullable=false)]
    public TestRunTestEntriesTestEntry[] TestEntries {
        get {
            return this.testEntriesField;
        }
        set {
            this.testEntriesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("UnitTestResult", typeof(TestRunResultsUnitTestResult), IsNullable=false)]
    public TestRunResultsUnitTestResult[] Results {
        get {
            return this.resultsField;
        }
        set {
            this.resultsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string runUser {
        get {
            return this.runUserField;
        }
        set {
            this.runUserField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunTestSettings {
    
    private TestRunTestSettingsDeployment[] deploymentField;
    
    private Execution[] executionField;
    
    private string nameField;
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Deployment")]
    public TestRunTestSettingsDeployment[] Deployment {
        get {
            return this.deploymentField;
        }
        set {
            this.deploymentField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Execution")]
    public Execution[] Execution {
        get {
            return this.executionField;
        }
        set {
            this.executionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunTestSettingsDeployment {
    
    private string userDeploymentRootField;
    
    private string useDefaultDeploymentRootField;
    
    private string runDeploymentRootField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string userDeploymentRoot {
        get {
            return this.userDeploymentRootField;
        }
        set {
            this.userDeploymentRootField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string useDefaultDeploymentRoot {
        get {
            return this.useDefaultDeploymentRootField;
        }
        set {
            this.useDefaultDeploymentRootField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string runDeploymentRoot {
        get {
            return this.runDeploymentRootField;
        }
        set {
            this.runDeploymentRootField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class Execution {
    
    private string testTypeSpecificField;
    
    private ExecutionAgentRule[] agentRuleField;
    
    private string idField;
    
    /// <remarks/>
    public string TestTypeSpecific {
        get {
            return this.testTypeSpecificField;
        }
        set {
            this.testTypeSpecificField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("AgentRule")]
    public ExecutionAgentRule[] AgentRule {
        get {
            return this.agentRuleField;
        }
        set {
            this.agentRuleField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class ExecutionAgentRule {
    
    private string nameField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunTimes {
    
    private string creationField;
    
    private string queuingField;
    
    private string startField;
    
    private string finishField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string creation {
        get {
            return this.creationField;
        }
        set {
            this.creationField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string queuing {
        get {
            return this.queuingField;
        }
        set {
            this.queuingField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string start {
        get {
            return this.startField;
        }
        set {
            this.startField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string finish {
        get {
            return this.finishField;
        }
        set {
            this.finishField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunResultSummary {
    
    private TestRunResultSummaryCounters[] countersField;
    
    private TestRunResultSummaryRunInfosRunInfo[] runInfosField;
    
    private string outcomeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Counters")]
    public TestRunResultSummaryCounters[] Counters {
        get {
            return this.countersField;
        }
        set {
            this.countersField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("RunInfo", typeof(TestRunResultSummaryRunInfosRunInfo), IsNullable=false)]
    public TestRunResultSummaryRunInfosRunInfo[] RunInfos {
        get {
            return this.runInfosField;
        }
        set {
            this.runInfosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string outcome {
        get {
            return this.outcomeField;
        }
        set {
            this.outcomeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunResultSummaryCounters {
    
    private string totalField;
    
    private string executedField;
    
    private string passedField;
    
    private string errorField;
    
    private string failedField;
    
    private string timeoutField;
    
    private string abortedField;
    
    private string inconclusiveField;
    
    private string passedButRunAbortedField;
    
    private string notRunnableField;
    
    private string notExecutedField;
    
    private string disconnectedField;
    
    private string warningField;
    
    private string completedField;
    
    private string inProgressField;
    
    private string pendingField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string total {
        get {
            return this.totalField;
        }
        set {
            this.totalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string executed {
        get {
            return this.executedField;
        }
        set {
            this.executedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string passed {
        get {
            return this.passedField;
        }
        set {
            this.passedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string error {
        get {
            return this.errorField;
        }
        set {
            this.errorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string failed {
        get {
            return this.failedField;
        }
        set {
            this.failedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string timeout {
        get {
            return this.timeoutField;
        }
        set {
            this.timeoutField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string aborted {
        get {
            return this.abortedField;
        }
        set {
            this.abortedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string inconclusive {
        get {
            return this.inconclusiveField;
        }
        set {
            this.inconclusiveField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string passedButRunAborted {
        get {
            return this.passedButRunAbortedField;
        }
        set {
            this.passedButRunAbortedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string notRunnable {
        get {
            return this.notRunnableField;
        }
        set {
            this.notRunnableField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string notExecuted {
        get {
            return this.notExecutedField;
        }
        set {
            this.notExecutedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string disconnected {
        get {
            return this.disconnectedField;
        }
        set {
            this.disconnectedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string warning {
        get {
            return this.warningField;
        }
        set {
            this.warningField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string completed {
        get {
            return this.completedField;
        }
        set {
            this.completedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string inProgress {
        get {
            return this.inProgressField;
        }
        set {
            this.inProgressField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string pending {
        get {
            return this.pendingField;
        }
        set {
            this.pendingField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunResultSummaryRunInfosRunInfo {
    
    private string textField;
    
    private string computerNameField;
    
    private string outcomeField;
    
    private string timestampField;
    
    /// <remarks/>
    public string Text {
        get {
            return this.textField;
        }
        set {
            this.textField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string computerName {
        get {
            return this.computerNameField;
        }
        set {
            this.computerNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string outcome {
        get {
            return this.outcomeField;
        }
        set {
            this.outcomeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string timestamp {
        get {
            return this.timestampField;
        }
        set {
            this.timestampField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunTestDefinitionsUnitTest {
    
    private TestRunTestDefinitionsUnitTestTestCategoryTestCategoryItem[] testCategoryField;
    
    private Execution[] executionField;
    
    private TestRunTestDefinitionsUnitTestTestMethod[] testMethodField;
    
    private string nameField;
    
    private string storageField;
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("TestCategoryItem", typeof(TestRunTestDefinitionsUnitTestTestCategoryTestCategoryItem), IsNullable=false)]
    public TestRunTestDefinitionsUnitTestTestCategoryTestCategoryItem[] TestCategory {
        get {
            return this.testCategoryField;
        }
        set {
            this.testCategoryField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Execution")]
    public Execution[] Execution {
        get {
            return this.executionField;
        }
        set {
            this.executionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("TestMethod")]
    public TestRunTestDefinitionsUnitTestTestMethod[] TestMethod {
        get {
            return this.testMethodField;
        }
        set {
            this.testMethodField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string storage {
        get {
            return this.storageField;
        }
        set {
            this.storageField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunTestDefinitionsUnitTestTestCategoryTestCategoryItem {
    
    private string testCategoryField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string TestCategory {
        get {
            return this.testCategoryField;
        }
        set {
            this.testCategoryField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunTestDefinitionsUnitTestTestMethod {
    
    private string codeBaseField;
    
    private string adapterTypeNameField;
    
    private string classNameField;
    
    private string nameField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string codeBase {
        get {
            return this.codeBaseField;
        }
        set {
            this.codeBaseField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string adapterTypeName {
        get {
            return this.adapterTypeNameField;
        }
        set {
            this.adapterTypeNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string className {
        get {
            return this.classNameField;
        }
        set {
            this.classNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunTestListsTestList {
    
    private string nameField;
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunTestEntriesTestEntry {
    
    private string testIdField;
    
    private string executionIdField;
    
    private string testListIdField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string testId {
        get {
            return this.testIdField;
        }
        set {
            this.testIdField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string executionId {
        get {
            return this.executionIdField;
        }
        set {
            this.executionIdField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string testListId {
        get {
            return this.testListIdField;
        }
        set {
            this.testListIdField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunResultsUnitTestResult {
    
    private TestRunResultsUnitTestResultOutput[] outputField;
    
    private string executionIdField;
    
    private string testIdField;
    
    private string testNameField;
    
    private string computerNameField;
    
    private string durationField;
    
    private string startTimeField;
    
    private string endTimeField;
    
    private string testTypeField;
    
    private string outcomeField;
    
    private string testListIdField;
    
    private string relativeResultsDirectoryField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Output")]
    public TestRunResultsUnitTestResultOutput[] Output {
        get {
            return this.outputField;
        }
        set {
            this.outputField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string executionId {
        get {
            return this.executionIdField;
        }
        set {
            this.executionIdField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string testId {
        get {
            return this.testIdField;
        }
        set {
            this.testIdField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string testName {
        get {
            return this.testNameField;
        }
        set {
            this.testNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string computerName {
        get {
            return this.computerNameField;
        }
        set {
            this.computerNameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string duration {
        get {
            return this.durationField;
        }
        set {
            this.durationField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string startTime {
        get {
            return this.startTimeField;
        }
        set {
            this.startTimeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string endTime {
        get {
            return this.endTimeField;
        }
        set {
            this.endTimeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string testType {
        get {
            return this.testTypeField;
        }
        set {
            this.testTypeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string outcome {
        get {
            return this.outcomeField;
        }
        set {
            this.outcomeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string testListId {
        get {
            return this.testListIdField;
        }
        set {
            this.testListIdField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string relativeResultsDirectory {
        get {
            return this.relativeResultsDirectoryField;
        }
        set {
            this.relativeResultsDirectoryField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunResultsUnitTestResultOutput {
    
    private string stdOutField;
    
    private TestRunResultsUnitTestResultOutputErrorInfo[] errorInfoField;
    
    /// <remarks/>
    public string StdOut {
        get {
            return this.stdOutField;
        }
        set {
            this.stdOutField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ErrorInfo")]
    public TestRunResultsUnitTestResultOutputErrorInfo[] ErrorInfo {
        get {
            return this.errorInfoField;
        }
        set {
            this.errorInfoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public partial class TestRunResultsUnitTestResultOutputErrorInfo {
    
    private string messageField;
    
    private string stackTraceField;
    
    /// <remarks/>
    public string Message {
        get {
            return this.messageField;
        }
        set {
            this.messageField = value;
        }
    }
    
    /// <remarks/>
    public string StackTrace {
        get {
            return this.stackTraceField;
        }
        set {
            this.stackTraceField = value;
        }
    }
}
namespace MSTestResultViewer.Consol
{
    
    
    public partial class NewDataSet {
    }
}
