using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine.Scripting;
#nullable enable
namespace Domain.Entities
{
  public class DeviceEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("module")]
    public ModuleEntity? ModuleEntity { get; set; }
    // [JsonProperty("JB",NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;
        // Basic Information
        [JsonProperty("function")] public string? Function { get; set; }
        [JsonProperty("type")] public string? Type { get; set; }
        [JsonProperty("ioAddress")] public string? IOAddress { get; set; }
        [JsonProperty("modelSeries")] public string? ModelSeries { get; set; }
        [JsonProperty("manufacturer")] public string? Manufacturer { get; set; }
        [JsonProperty("partNumber")] public string? PartNumber { get; set; }
        [JsonProperty("serialNumber")] public string? SerialNumber { get; set; }
        [JsonProperty("manufacturingYear")] public string? ManufacturingYear { get; set; }
        [JsonProperty("installationDate")] public string? InstallationDate { get; set; }

        // Technical Information
        [JsonProperty("measurementType")] public string? MeasurementType { get; set; }
        [JsonProperty("range")] public string? Range { get; set; }
        [JsonProperty("unit")] public string? Unit { get; set; }
        [JsonProperty("accuracy")] public string? Accuracy { get; set; }
        [JsonProperty("testError")] public string TestError { get; set; }
        [JsonProperty("lengthOrDN")] public string LengthOrDN { get; set; }
        [JsonProperty("supplyVoltage")] public string? SupplyVoltage { get; set; }
        [JsonProperty("outputSignal")] public string? OutputSignal { get; set; }
        [JsonProperty("ingressProtection")] public string? IngressProtection { get; set; }
        [JsonProperty("connectorType")] public string? ConnectorType { get; set; }
        [JsonProperty("processConnection")] public string? ProcessConnection { get; set; }
        [JsonProperty("responseTime")] public string? ResponseTime { get; set; }
        [JsonProperty("otherSpecifications")] public string? OtherSpecifications { get; set; }

        // Additional Information
        // [JsonProperty("additionalConnectionImages", NullValueHandling = NullValueHandling.Ignore)]
        [JsonProperty("additionalConnectionImages")]
    public List<ImageEntity>? AdditionalConnectionImageEntities { get; set; }
    [JsonProperty("jBs")]
    public List<JBEntity>? JBEntities { get; set; }


    public bool ShouldSerializeId()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription(),
      };
      return !apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeCode()
    {

      return true;
    }
    public bool ShouldSerializeFunction()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }
    public bool ShouldSerializeRange()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }
    public bool ShouldSerializeUnit()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }
    public bool ShouldSerializeIOAddress()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }

    public bool ShouldSerializeModuleEntity()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }

    public bool ShouldSerializeJBEntities()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeAdditionalConnectionImageEntities()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }



    [Preserve]
    public DeviceEntity()
    {
      // AdditionalConnectionImageEntities = new List<ImageEntity>();
    }

    [Preserve]
    public DeviceEntity(int id, string code)
    {
      Id = id;
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
    }

    [Preserve]

    public DeviceEntity(string code)
    {
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
    }


    [Preserve]
    public DeviceEntity(int id, string code, string function, string type, string modelSeries, string manufacturer, string partNumber, string serialNumber, string manufacturingYear, string installationDate, string ioAddress, string measurementType, string range, string unit, string accuracy, string testError, string lengthOrDN, string supplyVoltage, string outputSignal, string ingressProtection, string connectorType, string processConnection, string responseTime, string otherSpecifications, ModuleEntity? moduleEntity, List<JBEntity>? jbEntities, List<ImageEntity>? additionalConnectionImageEntities)
    {
      Id = id;
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
            Function = string.IsNullOrEmpty(function) ? "Chưa cập nhật" : function;
            Type = string.IsNullOrEmpty(type) ? "Chưa cập nhật" : type;
            IOAddress = string.IsNullOrEmpty(ioAddress) ? "Chưa cập nhật" : ioAddress;           
            ModelSeries = string.IsNullOrEmpty(modelSeries) ? "Chưa cập nhật" : modelSeries;
            Manufacturer = string.IsNullOrEmpty(manufacturer) ? "Chưa cập nhật" : manufacturer;
            PartNumber = string.IsNullOrEmpty(partNumber) ? "Chưa cập nhật" : partNumber;
            SerialNumber = string.IsNullOrEmpty(serialNumber) ? "Chưa cập nhật" : serialNumber;
            ManufacturingYear = string.IsNullOrEmpty(manufacturingYear) ? "Chưa cập nhật" : manufacturingYear;
            InstallationDate = string.IsNullOrEmpty(installationDate) ? "Chưa cập nhật" : installationDate;
            MeasurementType = string.IsNullOrEmpty(measurementType) ? "Chưa cập nhật" : measurementType;
            Range = string.IsNullOrEmpty(range) ? "Chưa cập nhật" : range;
            Unit = string.IsNullOrEmpty(unit) ? "Chưa cập nhật" : unit;
            Accuracy = string.IsNullOrEmpty(accuracy) ? "Chưa cập nhật" : accuracy;
            TestError = string.IsNullOrEmpty(testError) ? "Chưa cập nhật" : testError;
            LengthOrDN = string.IsNullOrEmpty(lengthOrDN) ? "Chưa cập nhật" : lengthOrDN;
            SupplyVoltage = string.IsNullOrEmpty(supplyVoltage) ? "Chưa cập nhật" : supplyVoltage;
            OutputSignal = string.IsNullOrEmpty(outputSignal) ? "Chưa cập nhật" : outputSignal;
            IngressProtection = string.IsNullOrEmpty(ingressProtection) ? "Chưa cập nhật" : ingressProtection;
            ConnectorType = string.IsNullOrEmpty(connectorType) ? "Chưa cập nhật" : connectorType;
            ProcessConnection = string.IsNullOrEmpty(processConnection) ? "Chưa cập nhật" : processConnection;
            ResponseTime = string.IsNullOrEmpty(responseTime) ? "Chưa cập nhật" : responseTime;
            OtherSpecifications = otherSpecifications;
            ModuleEntity = moduleEntity ?? null;
      JBEntities = (jbEntities == null || (jbEntities != null && !jbEntities.Any())) ? new List<JBEntity>() : jbEntities;
      AdditionalConnectionImageEntities = (additionalConnectionImageEntities == null
         || (additionalConnectionImageEntities != null && !additionalConnectionImageEntities.Any()))
         ? new List<ImageEntity>() : additionalConnectionImageEntities;
    }
    [Preserve]
        public DeviceEntity(string code, string function, string type, string modelSeries, string manufacturer, string partNumber, string serialNumber, string manufacturingYear, string installationDate, string ioAddress, string measurementType, string range, string unit, string accuracy, string testError, string lengthOrDN, string supplyVoltage, string outputSignal, string ingressProtection, string connectorType, string processConnection, string responseTime, string otherSpecifications, ModuleEntity? moduleEntity, List<JBEntity>? jbEntities, List<ImageEntity>? additionalConnectionImageEntities)
        {
            Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
            Function = string.IsNullOrEmpty(function) ? "Chưa cập nhật" : function;
            Type = string.IsNullOrEmpty(type) ? "Chưa cập nhật" : type;
            IOAddress = string.IsNullOrEmpty(ioAddress) ? "Chưa cập nhật" : ioAddress;            
            ModelSeries = string.IsNullOrEmpty(modelSeries) ? "Chưa cập nhật" : modelSeries;
            Manufacturer = string.IsNullOrEmpty(manufacturer) ? "Chưa cập nhật" : manufacturer;
            PartNumber = string.IsNullOrEmpty(partNumber) ? "Chưa cập nhật" : partNumber;
            SerialNumber = string.IsNullOrEmpty(serialNumber) ? "Chưa cập nhật" : serialNumber;
            ManufacturingYear = string.IsNullOrEmpty(manufacturingYear) ? "Chưa cập nhật" : manufacturingYear;
            InstallationDate = string.IsNullOrEmpty(installationDate) ? "Chưa cập nhật" : installationDate;
            MeasurementType = string.IsNullOrEmpty(measurementType) ? "Chưa cập nhật" : measurementType;
            Range = string.IsNullOrEmpty(range) ? "Chưa cập nhật" : range;
            Unit = string.IsNullOrEmpty(unit) ? "Chưa cập nhật" : unit;
            Accuracy = string.IsNullOrEmpty(accuracy) ? "Chưa cập nhật" : accuracy;
            TestError = string.IsNullOrEmpty(testError) ? "Chưa cập nhật" : testError;
            LengthOrDN = string.IsNullOrEmpty(lengthOrDN) ? "Chưa cập nhật" : lengthOrDN;
            SupplyVoltage = string.IsNullOrEmpty(supplyVoltage) ? "Chưa cập nhật" : supplyVoltage;
            OutputSignal = string.IsNullOrEmpty(outputSignal) ? "Chưa cập nhật" : outputSignal;
            IngressProtection = string.IsNullOrEmpty(ingressProtection) ? "Chưa cập nhật" : ingressProtection;
            ConnectorType = string.IsNullOrEmpty(connectorType) ? "Chưa cập nhật" : connectorType;
            ProcessConnection = string.IsNullOrEmpty(processConnection) ? "Chưa cập nhật" : processConnection;
            ResponseTime = string.IsNullOrEmpty(responseTime) ? "Chưa cập nhật" : responseTime;
            OtherSpecifications = string.IsNullOrEmpty(otherSpecifications) ? "Chưa cập nhật" : otherSpecifications;
            ModuleEntity = moduleEntity ?? null;
            JBEntities = (jbEntities == null || (jbEntities != null && !jbEntities.Any())) ? new List<JBEntity>() : jbEntities;
            AdditionalConnectionImageEntities = (additionalConnectionImageEntities == null
               || (additionalConnectionImageEntities != null && !additionalConnectionImageEntities.Any()))
               ? new List<ImageEntity>() : additionalConnectionImageEntities;
        }
    }
}