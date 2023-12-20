using Newtonsoft.Json.Linq;
using NvAPIWrapper;
using NvAPIWrapper.Display;
using NvAPIWrapper.GPU;

namespace SystemBridgeWindowsSensors
{
  public class Nvidia
  {
    public Nvidia()
    {
    }

    public JObject GetData()
    {
      NVIDIA.Initialize();

      JObject chipset = new()
      {
        ["id"] = NVIDIA.ChipsetInfo.DeviceId,
        ["name"] = NVIDIA.ChipsetInfo.ChipsetName,
        ["flags"] = NVIDIA.ChipsetInfo.Flags.ToString(),
        ["vendor_id"] = NVIDIA.ChipsetInfo.VendorId,
        ["vendor_name"] = NVIDIA.ChipsetInfo.VendorName,
      };

      JArray displaysArr = new();
      foreach (Display display in Display.GetDisplays())
      {
        displaysArr.Add(new JObject
        {
          ["id"] = display.DisplayDevice.DisplayId,
          ["name"] = display.Name,
          ["active"] = display.DisplayDevice.IsActive,
          ["available"] = display.DisplayDevice.IsAvailable,
          ["connected"] = display.DisplayDevice.IsConnected,
          ["dynamic"] = display.DisplayDevice.IsDynamic,
          ["aspect_horizontal"] = display.DisplayDevice.CurrentTiming.Extra.HorizontalAspect,
          ["aspect_vertical"] = display.DisplayDevice.CurrentTiming.Extra.VerticalAspect,
          ["brightness_current"] = display.DigitalVibranceControl.CurrentLevel,
          ["brightness_default"] = display.DigitalVibranceControl.DefaultLevel,
          ["brightness_max"] = display.DigitalVibranceControl.MaximumLevel,
          ["brightness_min"] = display.DigitalVibranceControl.MinimumLevel,
          ["color_depth"] = display.DisplayDevice.CurrentColorData.ColorDepth.Value.ToString(),
          ["connection_type"] = display.DisplayDevice.ConnectionType.ToString(),
          ["pixel_clock"] = display.DisplayDevice.CurrentTiming.PixelClockIn10KHertz,
          ["refresh_rate"] = display.DisplayDevice.CurrentTiming.Extra.RefreshRate,
          ["resolution_horizontal"] = display.DisplayDevice.CurrentTiming.HorizontalVisible,
          ["resolution_vertical"] = display.DisplayDevice.CurrentTiming.VerticalVisible,
        });
      }

      JObject driver = new()
      {
        ["branch_version"] = NVIDIA.DriverBranchVersion,
        ["interface_version"] = NVIDIA.InterfaceVersionString,
        ["version"] = NVIDIA.DriverVersion,
      };

      JArray gpusArr = [];
      foreach (PhysicalGPU gpu in PhysicalGPU.GetPhysicalGPUs())
      {
        JObject gpuObj = new();
        try { gpuObj["id"] = gpu.GPUId; } catch { }
        try { gpuObj["name"] = gpu.FullName; } catch { }
        try { gpuObj["bios_oem_revision"] = gpu.Bios.OEMRevision; } catch { }
        try { gpuObj["bios_revision"] = gpu.Bios.Revision; } catch { }
        try { gpuObj["bios_version"] = gpu.Bios.VersionString; } catch { }
        try { gpuObj["current_fan_speed_level"] = gpu.CoolerInformation.CurrentFanSpeedLevel; } catch { }
        try { gpuObj["current_fan_speed_rpm"] = gpu.CoolerInformation.CurrentFanSpeedInRPM; } catch { }
        try { gpuObj["current_temperature"] = gpu.ThermalInformation.CurrentThermalLevel; } catch { }
        try { gpuObj["driver_model"] = gpu.DriverModel; } catch { }
        try { gpuObj["memory_available"] = gpu.MemoryInformation.CurrentAvailableDedicatedVideoMemoryInkB; } catch { }
        try { gpuObj["memory_capacity"] = gpu.MemoryInformation.DedicatedVideoMemoryInkB; } catch { }
        try { gpuObj["memory_maker"] = gpu.MemoryInformation.RAMMaker.ToString(); } catch { }
        try { gpuObj["serial"] = gpu.Board.SerialNumber; } catch { }
        try { gpuObj["system_type"] = gpu.SystemType.ToString(); } catch { }
        try { gpuObj["type"] = gpu.GPUType.ToString(); } catch { }

        gpusArr.Add(gpuObj);
      }


      return new JObject
      {
        ["chipset"] = chipset,
        ["displays"] = displaysArr,
        ["driver"] = driver,
        ["gpus"] = gpusArr,
      };
    }

  }
}
