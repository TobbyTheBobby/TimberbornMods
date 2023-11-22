using Timberborn.WindSystem;

namespace ChooChoo
{
    public class StaticWindService
    {
        public static WindService WindService { get; private set; }
        
        StaticWindService(WindService windService)
        {
            WindService = windService;
        }
    }
}