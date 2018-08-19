using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using RGB.NET.Core;
using RGB.NET.Brushes;
using RGB.NET.Devices.Asus;
using RGB.NET.Devices.CoolerMaster;
using RGB.NET.Devices.Corsair;
using RGB.NET.Devices.Razer;
using RGB.NET.Devices.Logitech;
using RGB.NET.Groups;

namespace NodeRGBNet
{
    enum LedEffect
    {
        Static = 0,
        Ripple = 1,
        Rainbow = 2,
        Breath = 3
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("No valid argument.");
            }

            var arguments = new ExecArguments(args);
            var output = new Output();

            try
            {
                RGBSurface surface = RGBSurface.Instance;
                

                surface.AlignDevices();
                switch (arguments.action.ToLowerInvariant())
                {
                    case "--getdevices":
                        output.data = GetDevices(surface, arguments.deviceType);
                        break;
                    case "--setcolor":
                        output.data = SetColor(surface, arguments.deviceType, arguments.color);
                        break;
                    default:
                        output.error = "No valid argument.";
                        break;
                }
            }
            catch (Exception e)
            {
                output.error = e.ToString();
                output.data = null;
            }
            finally
            {
                Console.WriteLine(JsonConvert.SerializeObject(output));
            }
        }

        public static List<Device> GetDevices(RGBSurface surface, string type)
        {
            IList<IRGBDevice> devices = null;
            RGBDeviceType deviceType = 0;
            switch (type)
            {
                case "keyboard":
                    deviceType = RGBDeviceType.Keyboard;
                    break;
                case "mouse":
                    deviceType = RGBDeviceType.Mouse;
                    break;
                case "ledstripe":
                    deviceType = RGBDeviceType.LedStripe;
                    break;
                case "graphiccard":
                    deviceType = RGBDeviceType.GraphicsCard;
                    break;
                case "motherboard":
                    deviceType = RGBDeviceType.Mainboard;
                    break;
                default:
                    deviceType = RGBDeviceType.All;
                    break;
            }
            //string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            //Console.WriteLine(exeDir);
            //CoolerMasterDeviceProvider.PossibleX64NativePaths.Add(exeDir + "\\x64\\CMSDK.dll");
            //CoolerMasterDeviceProvider.PossibleX86NativePaths.Add(exeDir + "\\x86\\CMSDK.dll");
            AddDllPaths();
            surface.LoadDevices(new CorsairDeviceProviderLoader().GetDeviceProvider(), deviceType);
            surface.LoadDevices(new CoolerMasterDeviceProviderLoader().GetDeviceProvider(), deviceType);
            surface.LoadDevices(new LogitechDeviceProviderLoader().GetDeviceProvider(), deviceType);
            surface.LoadDevices(new RazerDeviceProviderLoader().GetDeviceProvider(), deviceType);
            devices = surface.GetDevices(deviceType);

            var data = devices
                          .Select(device => new Device()
                          {
                              id = device.DeviceInfo.Lighting,
                              type = type,
                              manufacturer = device.DeviceInfo.Manufacturer,
                              model = device.DeviceInfo.Model,
                          }).ToList();
            return data;
        }

        public static bool SetColor(RGBSurface surface, string type, Color color, LedEffect effect = 0)
        {
            IList<Device> devices = GetDevices(surface, type);
            try
            {
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                ledGroup.Brush = new SolidColorBrush(color);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static void AddDllPaths()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            AsusDeviceProvider.PossibleX86NativePaths.Add(exeDir + "\\86\\x86/AURA_SDK.dll");

            CoolerMasterDeviceProvider.PossibleX64NativePaths.Add(exeDir + "\\x64\\CMSDK.dll");
            CoolerMasterDeviceProvider.PossibleX86NativePaths.Add(exeDir + "\\x86\\CMSDK.dll");

            CorsairDeviceProvider.PossibleX64NativePaths.Add(exeDir + "\\x64\\CMSDK.dll");
            CorsairDeviceProvider.PossibleX86NativePaths.Add(exeDir + "\\x86\\CMSDK.dll");

            LogitechDeviceProvider.PossibleX64NativePaths.Add(exeDir + "\\x64\\RzChromaSDK.dll");
            LogitechDeviceProvider.PossibleX64NativePaths.Add(exeDir + "\\x64\\RzChromaSDK64.dll");
            LogitechDeviceProvider.PossibleX86NativePaths.Add(exeDir + "\\x86\\RzChromaSDK.dll");

            LogitechDeviceProvider.PossibleX64NativePaths.Add(exeDir + "\\x64\\LogitechLedEnginesWrapper.dll");
            LogitechDeviceProvider.PossibleX86NativePaths.Add(exeDir + "\\x86\\LogitechLedEnginesWrapper.dll");

        }

    }


    public class ExecArguments
    {
        public string action { get; set; }
        public string deviceType { get; set; }
        public Color color { get; set; }

        public ExecArguments(string[] args)
        {
            this.action = args[0];
            this.deviceType = args.Length > 1 ? args[1] : "all";
            this.color = args.Length > 2 ? hexToRGBA(args[2]) : new Color(0, 0, 0);
        }

        private Color hexToRGBA(string color)
        {
            string hexColor = color.Replace("#", "");
            int r = int.Parse(hexColor.Substring(0, 2),
                         System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(hexColor.Substring(2, 2),
                         System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(hexColor.Substring(4, 2),
                         System.Globalization.NumberStyles.HexNumber);
            return new Color(r, g, b);
        }
    }

    public class Device
    {
        public object id { get; set; }

        public string type { get; set; }

        public string manufacturer { get; set; }

        public string model { get; set; }

        public string led { get; set; }
    }

    public class Output
    {
        public object data { get; set; }

        public string error { get; set; }
    }
}
