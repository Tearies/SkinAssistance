namespace SkinAssistance.Core.Parameter
{

    public class AddInParameter : ParameterBase
    {
        private string _newPlayerIdentfier;

        #region Properties
        [Parameter("I", "EventName", true)]
        public string IpcName { get; set; }

        [Parameter("V", "version", true)]
        public string PluginVersion { get; set; }

        [Parameter("P", "param", true)]
        public string PluginParameter { get; set; }

        #endregion

        #region Overrides of ParameterBase

        /// <summary>
        /// 参数实例标识
        /// </summary>
        protected override string NewPlayerIdentfier
        {
            get { return "||"; }
        }



        #endregion
    }
}