using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin_Login
{
    public class SideNavMasterMenuItem
    {
        public SideNavMasterMenuItem()
        {
            TargetType = typeof(SideNavMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}