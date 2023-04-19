using System;
using System.Collections.Generic;

namespace ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        //Weapon();
        //Vehicle();

        Make01();

        Console.ReadKey();
    }

    static void Weapon()
    {
        string weaponJson = ".\\Database\\OriginWeapon.json";

        var getWeapons = JsonHelper.ReadFile<GetWeapons>(weaponJson);

        int index = 0;
        var temps = new List<Temp>();

        foreach (var res in getWeapons.result)
        {
            foreach (var wea in res.weapons)
            {
                //Console.WriteLine($"index: \t\t{++index}");
                //Console.WriteLine($"guid: \t\t{wea.guid}");
                //Console.WriteLine($"name: \t\t{wea.name}");
                //Console.WriteLine($"category: \t{wea.category}");
                //Console.WriteLine($"imageUrl: \t{wea.imageUrl}");
                //Console.WriteLine();

                temps.Add(new()
                {
                    index = ++index,
                    guid = wea.guid,
                    name = wea.name,
                    category = wea.category,
                    imageUrl = wea.imageUrl
                });
            }
        }

        JsonHelper.WriteFile("Weapon01.json", temps);
        Console.WriteLine("保存 Weapon01.json 成功");
    }

    static void Vehicle()
    {
        string vehicleJson = ".\\Database\\OriginVehicle.json";

        var getVehicles = JsonHelper.ReadFile<GetVehicles>(vehicleJson);

        int index = 0;
        var temps = new List<Temp>();

        foreach (var res in getVehicles.result)
        {
            foreach (var veh in res.vehicles)
            {
                //Console.WriteLine($"index: \t\t{++index}");
                //Console.WriteLine($"guid: \t\t{veh.guid}");
                //Console.WriteLine($"name: \t\t{veh.name}");
                //Console.WriteLine($"imageUrl: \t{veh.imageUrl}");
                //Console.WriteLine();

                temps.Add(new()
                {
                    index = ++index,
                    guid = veh.guid,
                    name = veh.name,
                    imageUrl = veh.imageUrl
                });
            }
        }

        JsonHelper.WriteFile("Vehicle01.json", temps);
        Console.WriteLine("保存 Vehicle01.json 成功");
    }

    static void Make01()
    {
        string all = ".\\Text\\All.json";

        var temps = JsonHelper.ReadFile<List<Temp>>(all);

        foreach (var item in WeaponDB.AllWeaponInfo)
        {
            var data = temps.Find(x => x.name.Contains(item.Chinese));
            if (data != null)
            {

            }
            else
            {
                Console.WriteLine($"未找到  {item.Chinese}");
            }

            //var data = temps.Find(x => x.imageUrl.Contains(item.ImageName));
            //if (data != null)
            //{

            //}
            //else
            //{
            //    Console.WriteLine($"未找到  {item.Chinese}");
            //}
        }
    }

    class Temp
    {
        public int index;
        public string guid;
        public string name;
        public string category;
        public string imageUrl;
    };
}
