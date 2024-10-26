using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Shared.Common
{
    public class GDUserClaimTypes
    {
        public const string UserName = "GD_UserName";
        public const string Email = "GD_Email";
        public const string Id = "GD_Id";
        public const string Roles = "GD_Roles";
    }

    public class GDOrderStatuses
    {
        public const string Selecting = "Сборка";
        public const string Waiting = "Ожидание";
        public const string InDelivery = "Доставляется";
        public const string Delivered = "Доставлен";
    }

    /// <summary>
    /// Наличка, Карта, Онлайн
    /// </summary>
    public class GDPayMethods
    {
        public const string Online = "online";
        public const string BankCard = "по карте";
        public const string Cash = "наличкой";
    }

    public static class Const
    {
		public static string BASE_URL = "https://gd9b3wbt-7265.euw.devtunnels.ms/";

	}

}
