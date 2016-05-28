using System;
using System.Collections.Generic;
using System.Globalization;
using MySql.Data.MySqlClient;
using NLog;

namespace WindowsFormsApplication1.Controller
{
    class SalesDbControl : DbControl
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void AddDiscounts(List<DiscountGoods> goods)
        {
            try
            {
                foreach (DiscountGoods product in goods)
                {
                    _logger.Debug("Attempt to add discount for " + product.Name);
                    Query("INSERT INTO `sales`(`discountid`, `barcode`, `name`, `discount`, `date`) VALUES(NULL, '" +
                          product.Barcode + "', '" + product.Name + "', '" + product.Discount + "', '" +
                          product.Date.ToString(new CultureInfo("ja-JP")) +
                          "')");
                    _logger.Debug("Attempt is successful");
                }
            }
            catch (Exception)
            {
                _logger.Debug("Attempt faild");
            }
        }

        public double GetDiscount(string barcode)
        {
            MySqlDataReader reader = ReadFrom("sales");
            _logger.Debug("Searching discount for " + barcode);
            while (reader.Read())
            {
            }
            {
                if (reader.GetInt32("barcode").ToString().Equals(barcode) &&
                    reader.GetDateTime("date").CompareTo(DateTime.Today) >= 0)
                {
                    _logger.Debug("Discount detected");
                    return 1 - (double) reader.GetInt32("discount")/100;
                }
            }
            _logger.Debug("Discount doesn`t exist");
            return 1;
        }
    }
}
