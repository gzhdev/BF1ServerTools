using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        //Weapon();
        //Vehicle();

        Make01();

        //Make02();
        //Make03();

        //Make04();

        Console.ReadKey();
    }

    static void Weapon()
    {
        string weaponJson = ".\\JSON\\OriginWeapon.json";

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
        string vehicleJson = ".\\JSON\\OriginVehicle.json";

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
        string all = ".\\TEXT\\All.json";

        var temps = JsonHelper.ReadFile<List<Temp>>(all);

        // new WeaponName(){ Kind="公用配枪", English="U_M1911", Chinese="M1911", ShortName="M1911", ImageName="Colt1911-ed324bf1.png" },

        foreach (var item in WeaponDB.AllWeaponInfo)
        {
            var data = temps.Find(x => x.imageUrl.Contains(item.ImageName));
            var build = new StringBuilder();

            build.Append("\t\t");
            build.Append("new WeaponName(){ ");

            if (data != null && !string.IsNullOrEmpty(item.ImageName))
            {
                build.Append($"Guid=\"{data.guid}\", ");
            }
            else
            {
                build.Append($"Guid=\"??????\", ");

                //Console.WriteLine($"未找到  {item.Chinese}");
            }

            build.Append($"Kind=\"{item.Kind}\", ");
            build.Append($"English=\"{item.English}\", ");
            build.Append($"Chinese=\"{item.Chinese}\", ");
            build.Append($"ShortName=\"{item.ShortName}\", ");
            build.Append($"ImageName=\"{item.ImageName}\" ");
            build.Append("},");

            Console.WriteLine(build.ToString());

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

    static void Make02()
    {
        string xmlPath = ".\\XML\\MPProfile.xml";

        var xml = new XmlDocument();
        xml.Load(xmlPath);

        var unlock = xml.SelectSingleNode("/PlayerTypeProfile/UnlockInfos");

        int index = 0;
        var temp2s = new List<Temp2>();

        foreach (XmlNode item in unlock.ChildNodes)
        {
            var basic = item.ChildNodes[0];

            var guid = basic.ChildNodes[0].InnerText;
            var id = basic.ChildNodes[1].InnerText;
            var name = basic.ChildNodes[8].InnerText;

            temp2s.Add(new()
            {
                index = ++index,
                guid = guid.ToUpper(),
                id = id.ToUpper(),
                name = name
            });
        }

        JsonHelper.WriteFile("MPUnlockInfos01.json", temp2s);
        Console.WriteLine("保存 MPUnlockInfos01.json 成功");
    }

    static void Make03()
    {
        string xmlPath = ".\\XML\\SPProfile.xml";

        var xml = new XmlDocument();
        xml.Load(xmlPath);

        var unlock = xml.SelectSingleNode("/PlayerTypeProfile/UnlockInfos");

        int index = 0;
        var temp2s = new List<Temp2>();

        foreach (XmlNode item in unlock.ChildNodes)
        {
            var basic = item.ChildNodes[0];

            var guid = basic.ChildNodes[0].InnerText;
            var id = basic.ChildNodes[1].InnerText;
            var name = basic.ChildNodes[8].InnerText;

            temp2s.Add(new()
            {
                index = ++index,
                guid = guid.ToUpper(),
                id = id.ToUpper(),
                name = name
            });
        }

        JsonHelper.WriteFile("SPUnlockInfos01.json", temp2s);
        Console.WriteLine("保存 SPUnlockInfos01.json 成功");
    }

    static void Make04()
    {
        string all = ".\\TEXT\\MPUnlockInfos01.json";

        var temp2s = JsonHelper.ReadFile<List<Temp2>>(all);

        // new WeaponName(){ Kind="公用配枪", English="U_M1911", Chinese="M1911", ShortName="M1911", ImageName="Colt1911-ed324bf1.png" },

        foreach (var item in WeaponDB.AllWeaponInfo)
        {
            var data = temp2s.Find(x => x.name == item.English);
            var build = new StringBuilder();

            build.Append("\t\t");
            build.Append("new WeaponName(){ ");


            if (data != null)
            {
                build.Append($"Guid=\"{data.guid}\", ");
            }
            else
            {
                build.Append($"Guid=\"????????\", ");

                //Console.WriteLine($"未找到  {item.Chinese}");
            }

            build.Append($"Kind=\"{item.Kind}\", ");
            build.Append($"English=\"{item.English}\", ");
            build.Append($"Chinese=\"{item.Chinese}\", ");
            build.Append($"ShortName=\"{item.ShortName}\", ");
            build.Append($"ImageName=\"{item.ImageName}\" ");
            build.Append("},");

            Console.WriteLine(build.ToString());

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

    class Temp2
    {
        public int index;
        public string guid;
        public string id;
        public string name;
    };
}
