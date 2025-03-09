using Microsoft.Extensions.Logging;

namespace RhymeBinder.Models.HelperModels
{
    public class ModelHelper
    {
        public BinderHelper BinderHelper { get; set; }
        public GroupHelper GroupHelper { get; set; }
        public TextHelper TextHelper { get; set; }
        public UserHelper UserHelper { get; set; }
        public ViewHelper ViewHelper { get; set; }

        // Constructor
        public ModelHelper(RhymeBinderContext context, ILogger<BaseHelper> logger)
        {
            BinderHelper = new BinderHelper(context, logger);
            GroupHelper = new GroupHelper(context, logger);
            TextHelper = new TextHelper(context, logger);
            UserHelper = new UserHelper(context, logger);
            ViewHelper = new ViewHelper(context, logger);
        }
    }
}