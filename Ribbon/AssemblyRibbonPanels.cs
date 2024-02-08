namespace Inventor.InternalNames.Ribbon
{
    public struct AssemblyRibbonPanels
    {
        public struct MoldLayoutTab
        {
            public const string MoldLayout = "MoldPanelLayout"; //Mold Layout
            public const string RunnersAndChannels = "MoldPanelRunnerChannel"; //Mold Layout
            public const string MoldSimulation = "MoldPanelSimulation"; //Mold Layout
            public const string Tools = "MoldPanelTools"; //Mold Layout
        }

        public struct CoreCavityTab
        {
            public const string PlasticPart = "MoldPanelPlasticPart"; //Core/Cavity
            public const string PartingDesign = "MoldPanelPartingDesign"; //Core/Cavity
            public const string Insert = "MoldPanelInsert"; //Core/Cavity
            public const string Tools = "MoldPanelTools"; //Core/Cavity
        }

        public struct MoldAssemblyTab
        {
            public const string MoldAssembly = "MoldPanelStandardComponents"; //Mold Assembly
            public const string Boolean = "MoldPanelMoldBoolean"; //Mold Assembly
            public const string k2DDrawing = "MoldPanel2DDrawing"; //Mold Assembly
            public const string Author = "MoldPanelAuthor"; //Mold Assembly
        }

        public struct AssembleTab
        {
            public const string Component = "id_PanelA_AssembleComponent"; //Assemble
            public const string Position = "id_PanelA_AssemblePosition"; //Assemble
            public const string Relationships = "id_PanelA_AssembleRelationships"; //Assemble
            public const string Pattern = "id_PanelA_AssemblePattern"; //Assemble
            public const string Manage = "id_PanelA_AssembleManage"; //Assemble
            public const string iPart_iAssembly = "id_PanelA_AssembleIPartIAssembly"; //Assemble
            public const string Productivity = "id_PanelA_AssembleAssemblyTools"; //Assemble
            public const string WorkFeatures = "id_PanelA_ModelWorkFeatures"; //Assemble
            public const string Simplification = "id_PanelAssm_Simplification"; //Assemble
            public const string Measure = "id_PanelA_ToolsMeasure"; //Assemble
            public const string ShowPanels = "id_PanelP_ShowPanels"; //Assemble
        }

        public struct DesignTab
        {
            public const string Fasten = "id_PanelA_DesignFasten"; //Design
            public const string Frame = "id_PanelA_DesignFrame"; //Design
            public const string PowerTransmission = "id_PanelA_DesignPowerTransmission"; //Design
            public const string Spring = "id_PanelA_DesignSpring"; //Design
            public const string Measure = "id_PanelA_ToolsMeasure"; //Design
            public const string Analyze = "id_PanelP_ToleranceAnalyze"; //Design
            public const string ShowPanels = "id_PanelP_ShowPanels"; //Design
        }

        public struct k3DModelTab
        {
            public const string Sketch = "id_PanelA_ModelSketch"; //3D Model
            public const string ModifyAssembly = "id_PanelA_ModelModify"; //3D Model
            public const string WorkFeatures = "id_PanelA_ModelWorkFeatures"; //3D Model
            public const string Pattern = "id_PanelA_ModelPattern"; //3D Model
            public const string Parameters = "id_PanelA_ModelManage"; //3D Model
            public const string Measure = "id_PanelA_ToolsMeasure"; //3D Model
            public const string ShowPanels = "id_PanelP_ShowPanels"; //3D Model
        }

        public struct SketchTab
        {
            public const string Sketch = "id_PanelA_ModelSketch"; //Sketch
            public const string Create = "id_PanelA_2DSketchDraw"; //Sketch
            public const string Modify = "id_PanelA_2DSketchModify"; //Sketch
            public const string Pattern = "id_PanelA_2DSketchPattern"; //Sketch
            public const string Constrain = "id_PanelA_2DSketchConstrain"; //Sketch
            public const string Insert = "id_PanelA_2DSketchInsert"; //Sketch
            public const string Format = "id_PanelA_2DSketchFormat"; //Sketch
            public const string Measure = "id_PanelA_ToolsMeasure"; //Sketch
            public const string ShowPanels = "id_PanelP_ShowPanels"; //Sketch
        }

        public struct AnnotateTab
        {
            public const string GeneralAnnotation = "id_PanelAdskMBDGeneralAnno"; //Annotate
            public const string Notes = "id_PanelAdskMBDNotes"; //Annotate
            public const string Manage = "id_PanelAdskMBDManage"; //Annotate
            public const string Analyze = "id_PanelAdskMBDAnalyze"; //Annotate
            public const string Export = "id_PanelAdskMBDExport"; //Annotate
        }

        public struct WeldTab
        {
            public const string Process = "id_PanelA_WeldProcess"; //Weld
            public const string Weld = "id_PanelA_WeldWeld"; //Weld
            public const string Sketch = "id_PanelA_WeldSketch"; //Weld
            public const string PreparationAndMachining = "id_PanelA_WeldModify"; //Weld
            public const string WorkFeatures = "id_PanelA_ModelWorkFeatures"; //Weld
            public const string Pattern = "id_PanelA_WeldPattern"; //Weld
            public const string Parameters = "id_PanelA_WeldManage"; //Weld
            public const string Measure = "id_PanelA_ToolsMeasure"; //Weld
            public const string ShowPanels = "id_PanelP_ShowPanels"; //Weld
        }

        public struct WeldReturnToParentTab
        {
            public const string Return = "id_PanelA_WeldReturnToParent"; //Weld Return to Parent
        }

        public struct InspectTab
        {
            public const string Interference = "id_PanelA_ToolsAnalysis"; //Inspect
            public const string Analyze = "id_PanelP_ToleranceAnalyze"; //Inspect
            public const string Measure = "id_PanelA_AnalysisMeasure"; //Inspect
            public const string AutoLimits = "id_PanelA_ToolsAutoLimits"; //Inspect
            public const string ShowPanels = "id_PanelP_ShowPanels"; //Inspect
        }

        public struct ToolsTab
        {
            public const string Measure = "id_PanelA_ToolsMeasure"; //Tools
            public const string MaterialAndAppearance = "id_PanelP_ToolsMaterial"; //Tools
            public const string Options = "id_PanelP_ToolsOptions"; //Tools
            public const string Clipboard = "id_PanelP_ToolsClipboard"; //Tools
            public const string Find = "id_PanelP_ToolsFind"; //Tools
            public const string ShowPanels = "id_PanelP_ShowPanels"; //Tools
        }

        public struct ManageTab
        {
            public const string Update = "id_PanelA_ToolsUpdate"; //Manage
            public const string Manage = "id_PanelA_AssembleManage"; //Manage
            public const string StylesAndStandards = "id_PanelA_ToolsStylesAndStandards"; //Manage
            public const string Insert = "id_PanelA_ToolsInsert"; //Manage
            public const string PointCloud = "id_PanelP_ToolsPointCloud"; //Manage
            public const string Author = "id_PanelA_ToolsAuthor"; //Manage
            public const string iLogic = "iLogic.RibbonPanel"; //Manage
            public const string ContentCenter = "id_PanelA_ToolsContentCenter"; //Manage
            public const string ShowPanels = "id_PanelP_ShowPanels"; //Manage
        }

        public struct ViewTab
        {
            public const string Visibility = "id_PanelA_ViewVisibility"; //View
            public const string Appearance = "id_PanelA_ViewAppearance"; //View
            public const string Windows = "id_PanelP_ViewWindow"; //View
            public const string Navigate = "id_PanelC_ViewNavigate"; //View
            public const string ShowPanels = "id_PanelP_ShowPanels"; //View
        }

        public struct EnvironmentsTab
        {
            public const string Begin = "id_Panel_EnvironmentsBegin"; //Environments
            public const string Convert = "id_PanelA_ConvertWeld"; //Environments
            public const string k3DPrint = "id_PanelP_3DPrint_Legacy"; //Environments
            public const string Manage = "id_PanelA_EnvironmentsAddIns"; //Environments
            public const string ShowPanels = "id_PanelP_ShowPanels"; //Environments
        }

        public struct DataStandardTab
        {
            public const string File = "AD:dataStandardAddIn:PanelAssemblyFile"; //Data Standard
            public const string Copy = "AD:dataStandardAddIn:PanelAssemblyCopy"; //Data Standard
            public const string Settings = "AD:dataStandardAddIn:PanelAssemblySettings"; //Data Standard
        }

        public struct GetStartedTab
        {
            public const string Launch = "id_Panel_Launch"; //Get Started
            public const string MyHome = "id_Panel_MyHome"; //Get Started
            public const string NewFeatures = "id_Panel_GetStartedWhatsNew"; //Get Started
            public const string Help = "AutodeskTutorialAddInHelpPanel"; //Get Started
            public const string ShowPanels = "id_PanelP_ShowPanels"; //Get Started
        }

        public struct VaultTab
        {
            public const string Access = "id_PanelZ_VaultAccess"; //Vault
            public const string FileStatus = "id_PanelZ_VaultStatusUpgrade"; //Vault
            public const string Control = "id_PanelZ_VaultControl"; //Vault
            public const string Find = "id_PanelZ_VaultFind"; //Vault
        }

        public struct CollaborateTab
        {
            public const string Share = "id_Panel_OnlineDocument"; //Collaborate
            public const string Web = "id_PanelP_ToolsWebTools"; //Collaborate
            public const string DataManagement = "id_PanelP_PDMAndPLM"; //Collaborate
            public const string ShowPanels = "id_PanelP_ShowPanels"; //Collaborate
        }

        public struct ElectromechanicalTab
        {
            public const string Setup = "id_PanelA_Mechatronics"; //Electromechanical
        }

        public struct Exit2DSketchTab
        {
            public const string Exit = "id_PanelA_2DSketchExit"; //Exit 2D Sketch
        }

        public struct ModeTab
        {
            public const string Express = "id_PanelA_AssembleExpress"; //Mode
        }

        public struct TubeAndPipeTab
        {
            public const string Run = "id_PanelA_TubePipeRoute"; //Tube and Pipe
            public const string Content = "id_PanelA_TubePipeContentCenter"; //Tube and Pipe
            public const string Component = "id_PanelA_TubePipeComponent"; //Tube and Pipe
            public const string Position = "id_PanelA_TubePipePosition"; //Tube and Pipe
            public const string Relationships = "id_PanelA_TubePipeRelationships"; //Tube and Pipe
            public const string Manage = "id_PanelA_TubePipeManage"; //Tube and Pipe
            public const string Parameters = "id_PanelP_ToolsParameters"; //Tube and Pipe
            public const string iPart_iAssembly = "id_PanelA_TubePipeIPartIAssembly"; //Tube and Pipe
        }

        public struct ExitTubeAndPipeTab
        {
            public const string Exit = "id_PanelA_TubePipeExit"; //Exit Tube and Pipe
        }

        public struct PipeRunTab
        {
            public const string Route = "id_PanelA_TubePipeRunCreate"; //Pipe Run
            public const string Content = "id_PanelA_TubePipeRunContentCenter"; //Pipe Run
            public const string Manage = "id_PanelA_TubePipeRunManage"; //Pipe Run
            public const string Parameters = "id_PanelP_ToolsParameters"; //Pipe Run
        }

        public struct ExitTubeAndPipeRunTab
        {
            public const string Exit = "id_PanelA_TubePipeRunExit"; //Exit Tube and Pipe Run
        }

        public struct CableAndHarnessTab
        {
            public const string Create = "id_PanelA_CableHarnessCreate"; //Cable and Harness
            public const string Route = "id_PanelA_CableHarnessRoute"; //Cable and Harness
            public const string Manage = "id_PanelA_CableHarnessManage"; //Cable and Harness
            public const string Visibility = "id_PanelA_CableHarnessVisibility"; //Cable and Harness
        }

        public struct ExitCableAndHarnessTab
        {
            public const string Exit = "id_PanelA_CableHarnessExit"; //Exit Cable and Harness
        }

        public struct RenderTab
        {
            public const string Render = "id_PanelA_StudioRender"; //Render
            public const string Scene = "id_PanelA_StudioScene"; //Render
            public const string Animate = "id_PanelA_StudioAnimate"; //Render
            public const string Manage = "id_PanelA_StudioManage"; //Render
        }

        public struct ExitStudioTab
        {
            public const string Exit = "id_PanelA_StudioExit"; //Exit Studio
        }

        public struct DynamicSimulationTab
        {
            public const string Joint = "id_PanelA_SimulationJoint"; //Dynamic Simulation
            public const string Load = "id_PanelA_SimulationLoad"; //Dynamic Simulation
            public const string Results = "id_PanelA_SimulationResults"; //Dynamic Simulation
            public const string Animate = "id_PanelA_SimulationAnimate"; //Dynamic Simulation
            public const string Manage = "id_PanelA_SimulationMaterial"; //Dynamic Simulation
            public const string StressAnalysis = "id_PanelA_SimulationExport"; //Dynamic Simulation
        }

        public struct ExitDynamicSimulationTab
        {
            public const string Exit = "id_PanelA_SimulationExit"; //Exit Dynamic Simulation
        }

        public struct ReturnTab
        {
            public const string Return = "id_PanelA_AssmReturn"; //Return
        }

        public struct AnalysisTab
        {
            public const string Manage = "id_PanelA_AFEAManage"; //Analysis
            public const string Material = "id_PanelA_AFEAMaterial"; //Analysis
            public const string Prepare = "id_PanelA_AFEAPrepare"; //Analysis
            public const string Constraints = "id_PanelA_AFEAConstraints"; //Analysis
            public const string Loads = "id_PanelA_AFEALoads"; //Analysis
            public const string GoalsAndCriteria = "id_PanelA_AFEAGoalsCriteria"; //Analysis
            public const string Contacts = "id_PanelA_AFEAContacts"; //Analysis
            public const string Mesh = "id_PanelA_AFEAMesh"; //Analysis
            public const string Run = "id_PanelA_AFEARun"; //Analysis
            public const string Solve = "id_PanelA_AFEASolve"; //Analysis
            public const string Result = "id_PanelA_AFEAResult"; //Analysis
            public const string Display = "id_PanelA_AFEAResultDisplay"; //Analysis
            public const string Export = "id_PanelA_AFEAExport"; //Analysis
            public const string Report = "id_PanelA_AFEAReport"; //Analysis
            public const string Guide = "id_PanelA_AFEAGuide"; //Analysis
            public const string Settings = "id_PanelA_AFEASettings"; //Analysis
        }

        public struct ExitStressAnalysisTab
        {
            public const string Exit = "id_PanelA_AFEAExit"; //Exit Stress Analysis
        }

        public struct FrameAnalysisTab
        {
            public const string Manage = "id_PanelA_IFAManage"; //Frame Analysis
            public const string Beams = "id_PanelA_IFAProperties"; //Frame Analysis
            public const string Constraints = "id_PanelA_IFAConstraints"; //Frame Analysis
            public const string Loads = "id_PanelA_IFA_Loads"; //Frame Analysis
            public const string Connections = "id_PanelA_IFA_Connections"; //Frame Analysis
            public const string Solve = "id_PanelA_IFA_Solve"; //Frame Analysis
            public const string Result = "id_PanelA_IFA_Result"; //Frame Analysis
            public const string Display = "id_PanelA_IFA_Display"; //Frame Analysis
            public const string Publish = "id_PanelA_IFA_Publish"; //Frame Analysis
            public const string Settings = "id_PanelA_IFA_Settings"; //Frame Analysis
        }

        public struct ExitFrameAnalysisTab
        {
            public const string Exit = "id_PanelA_IFAExit"; //Exit Frame Analysis
        }

        public struct BIMContentTab
        {
            public const string Simplify = "id_PanelA_AECExchangeSimplify"; //BIM Content
            public const string MEPConnectors = "id_PanelA_AECExchangeConnectors"; //BIM Content
            public const string Author = "id_PanelA_AECExchangeAuthor"; //BIM Content
            public const string Publish = "id_PanelA_AECExchangePublish"; //BIM Content
        }
    }
}