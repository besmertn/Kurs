using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MySql.Data.MySqlClient;
using NLog;

namespace WindowsFormsApplication1.Controller
{
    internal class GoodsDbControl : DbControl
    {
        readonly Dictionary<string, Goods> _goods = new Dictionary<string, Goods>();
        private readonly CultureInfo _clt = new CultureInfo("ja-JP");
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void FillList()
        {
            MySqlDataReader reader = new DbControl().ReadFrom("goods");
            _logger.Debug("Filling goods list");
            while (reader.Read())
            {
                _goods.Add(reader.GetInt32("barcode").ToString(), new Goods(reader.GetInt32("barcode").ToString()
                    , reader.GetString("name")
                    , reader.GetString("measure")
                    , reader.GetInt32("count")
                    , reader.GetFloat("price")
                    , reader.GetDateTime("lastdelivery")
                    , reader.GetDateTime("shelflife")));
            }
        }

        public void ClearList()
        {
            _goods.Clear();
        }

        public Goods SearchGoods(string requiredBarcode)
        {
            try
            {
                _logger.Debug("Searching for " + requiredBarcode);
                return _goods[requiredBarcode];
            }
            catch (KeyNotFoundException)
            {
                _logger.Warn("Such key not exist");
                return null;
            }
        }

        public void BuyGoods(List<Goods> purchase)
        {
            int tmp = Convert.ToInt32(Properties.Settings.Default.number) + 1;
            Properties.Settings.Default.number = tmp.ToString();
            foreach (Goods product in purchase)
            {
                Query("UPDATE `goods` SET `count` =  `count`-" + product.Count + " WHERE `barcode`= " + product.Barcode +
                      " ;");
                Query("INSERT INTO `purchase`(`id`, `barcode`, `name`, `amount`, `date`) VALUES('" +
                      Properties.Settings.Default.number + "', '" + product.Barcode + "', '" + product.Name + "', '" +
                      product.Count + "','" + DateTime.Now.ToString(_clt) + "');");
            }
        }

        public List<Goods> ShelfLifeControl()
        {
            _goods.Clear();
            FillList();
            Dictionary<string, Goods> tmpGoodsDictionary = _goods
                .Where(x => x.Value.ShelfLife.CompareTo(DateTime.Now) <= 0)
                .ToDictionary(key => key.Key, value => value.Value);
            List<Goods> pastDueGoods = tmpGoodsDictionary.Values.ToList();
            foreach (Goods product in pastDueGoods)
            {
                try
                {
                    _logger.Debug("Transfering INSERT INTO `stitchedgoods`");
                    Query("DELETE FROM `Kurs`.`goods` WHERE `goods`.`barcode` = " + product.Barcode + ";");
                    _goods.Remove(product.Barcode);
                    Query(
                        "INSERT INTO `stitchedgoods`(`barcode`, `name`, `measure`, `count`, `price`, `shelflife`) VALUES(" +
                        product.Barcode + ", '" + product.Name + "', '" + product.Measure + "', '" + product.Count +
                        "', '" + product.Price + "', '" + product.ShelfLife.ToString("d", _clt) + "')");
                    _logger.Debug("Successful SQL query");
                }
                catch (MySqlException e)
                {
                    if (e.ErrorCode == 1062)
                    {
                        _logger.Warn("Such key already exist, creating of new key");
                        Query(
                            "INSERT INTO `stitchedgoods`(`barcode`, `name`, `measure`, `count`, `price`, `shelflife`) VALUES(NULL, '" +
                            product.Name + "', '" + product.Measure + "', '" + product.Count + "', '" + product.Price +
                            "', '" + product.ShelfLife.ToString("d", _clt) + "')");
                        _logger.Debug("Successful SQL query");
                    }
                }
            }
            return pastDueGoods;
        }

        public Boolean NewDelivery(List<Goods> delivery)
        {
            foreach (Goods product in delivery)
            {
                try
                {
                    _logger.Debug("Trying INSERT INTO `goods`");
                    Query(
                        "INSERT INTO `goods` (`barcode`, `name`, `measure`, `count`, `price`, `lastdelivery`, `shelflife`) VALUES('" +
                        product.Barcode + "', '" + product.Name + "', '" + product.Measure + "', '" + product.Count +
                        "', '" + product.Price + "', '" + DateTime.Now.ToString("d", _clt) + "', '" +
                        product.ShelfLife.ToString("d", _clt) + "')");
                    _goods.Add(product.Barcode, new Goods(product.Barcode
                        , product.Name
                        , product.Measure
                        , product.Count
                        , product.Price
                        , product.LastDelivery
                        , product.ShelfLife));
                    _logger.Debug("Successful SQL query");
                }
                catch (MySqlException e)
                {
                    if (e.ErrorCode == 1062)
                    {
                        _logger.Warn("Such key already exist, creating of new key");
                        Query(
                            "INSERT INTO `Kurs`.`goods` (`barcode`, `name`, `measure`, `count`, `price`, `lastdelivery`, `shelflife`) VALUES(NULL, '" +
                            product.Name + "', '" + product.Measure + "'), '" +
                            product.Count + "', '" + product.Price + "', '" +
                            DateTime.Now.ToString("d", _clt) + "', '" +
                            product.ShelfLife.ToString("d", _clt) + "'");
                        _goods.Clear();
                        FillList();
                        _logger.Debug("Successful SQL query");
                    }
                }
                catch
                {
                    _logger.Error("SQL quey error");
                    return false;
                }
            }
            return true;
        }

        public int GetCheckCounter(int number)
        {
            _logger.Debug("Getting of checkcounter");
            MySqlDataReader reader =
                Query("SELECT `checkcounter` FROM `checkcounter` WHERE `cashregisternumber`=" + number + ";")
                    .ExecuteReader();
            reader.Read();
            _logger.Info("Current check number is " + reader.GetInt32("checkcounter"));
            return reader.GetInt32("checkcounter");
        }

        public void UpdateCheckCounter(int number)
        {
            _logger.Debug("Updating of checkcounter");
            MySqlDataReader reader =
                Query("SELECT `checkcounter` FROM `checkcounter` WHERE `cashregisternumber`=" + number + ";")
                    .ExecuteReader();
            while (reader.Read())
            {
                Query(
                    "UPDATE `Kurs`.`checkcounter` SET `checkcounter` = `checkcounter` + 1 WHERE `checkcounter`.`cashregisternumber` = " +
                    number + ";");
            }
        }
    }
}
