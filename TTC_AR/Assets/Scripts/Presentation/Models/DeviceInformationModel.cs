using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
using static ModuleInformationModel;
#nullable enable
[Preserve]
public class DeviceInformationModel
{
  [JsonProperty("id")]
  public int Id { get; set; }

  [JsonProperty("code")]
  public string Code { get; set; } = string.Empty;

  // Basic Information
  [JsonProperty("function")] public string Function { get; set; }
  [JsonProperty("ioAddress")] public string IOAddress { get; set; }
  [JsonProperty("type")] public string Type { get; set; }
  [JsonProperty("modelSeries")] public string ModelSeries { get; set; }
  [JsonProperty("manufacturer")] public string Manufacturer { get; set; }
  [JsonProperty("partNumber")] public string PartNumber { get; set; }
  [JsonProperty("serialNumber")] public string SerialNumber { get; set; }
  [JsonProperty("manufacturingYear")] public string ManufacturingYear { get; set; }
  [JsonProperty("installationDate")] public string InstallationDate { get; set; }

  // Technical Information
  [JsonProperty("measurementType")] public string MeasurementType { get; set; }
  [JsonProperty("range")] public string Range { get; set; }
  [JsonProperty("unit")] public string Unit { get; set; }
  [JsonProperty("accuracy")] public string Accuracy { get; set; }
  [JsonProperty("supplyVoltage")] public string SupplyVoltage { get; set; }
  [JsonProperty("outputSignal")] public string OutputSignal { get; set; }
  [JsonProperty("ingressProtection")] public string IngressProtection { get; set; }
  [JsonProperty("connectorType")] public string ConnectorType { get; set; }
  [JsonProperty("processConnection")] public string ProcessConnection { get; set; }
  [JsonProperty("responseTime")] public string ResponseTime { get; set; }
  [JsonProperty("otherSpecifications")] public string OtherSpecifications { get; set; }

  // Operational Information
  [JsonProperty("installationLocation")] public string InstallationLocation { get; set; }
  [JsonProperty("measuredMedium")] public string MeasuredMedium { get; set; }
  [JsonProperty("operatingTemperature")] public string OperatingTemperature { get; set; }
  [JsonProperty("operatingPressure")] public string OperatingPressure { get; set; }
  [JsonProperty("calibrationFrequency")] public string CalibrationFrequency { get; set; }
  [JsonProperty("failureHistory")] public string FailureHistory { get; set; }
  [JsonProperty("environmentCondition")] public string EnvironmentCondition { get; set; }

  // Additional Information
  [JsonProperty("module")]
  public ModuleInformationModel? ModuleInformationModel { get; set; }

  [JsonProperty("jBs")]
  public List<JBInformationModel>? JBInformationModels { get; set; }

  [JsonProperty("additionalConnectionImages")]
  public List<ImageInformationModel>? AdditionalConnectionImages { get; set; }

    [Preserve]

    public DeviceInformationModel(int id, string code, string? function, string? type, string? modelSeries, string? manufacturer, string? partNumber, string? serialNumber, string? manufacturingYear, string? installationDate, string? ioAddress, string measurementType, string? range, string? unit, string? accuracy, string? supplyVoltage, string? outputSignal, string? ingressProtection, string? connectorType, string? processConnection, string? responseTime, string? otherSpecifications, string? installationLocation, string? measuredMedium, string? operatingTemperature, string? operatingPressure, string? calibrationFrequency, string? failureHistory, string? environmentCondition, ModuleInformationModel? moduleInformationModel, List<JBInformationModel>? jbInformationModels, List<ImageInformationModel>? additionalConnectionImages)
    {
        Id = id;
        Code = code;
        Function = function;
        Type = type;
        ModelSeries = modelSeries;
        Manufacturer = manufacturer;
        PartNumber = partNumber;
        SerialNumber = serialNumber;
        ManufacturingYear = manufacturingYear;
        InstallationDate = installationDate;
        IOAddress = ioAddress;
        MeasurementType = measurementType;
        Range = range;
        Unit = unit;
        Accuracy = accuracy;
        SupplyVoltage = supplyVoltage;
        OutputSignal = outputSignal;
        IngressProtection = ingressProtection;
        ConnectorType = connectorType;
        ProcessConnection = processConnection;
        ResponseTime = responseTime;
        OtherSpecifications = otherSpecifications;
        InstallationLocation = installationLocation;
        MeasuredMedium = measuredMedium;
        OperatingTemperature = operatingTemperature;
        OperatingPressure = operatingPressure;
        CalibrationFrequency = calibrationFrequency;
        FailureHistory = failureHistory;
        EnvironmentCondition = environmentCondition;
        ModuleInformationModel = moduleInformationModel;
        JBInformationModels = jbInformationModels;
        AdditionalConnectionImages = additionalConnectionImages;
    }


  public DeviceInformationModel(string code, string? function, string? type, string? modelSeries, string? manufacturer, string? partNumber, string? serialNumber, string? manufacturingYear, string? installationDate, string? ioAddress, string measurementType, string? range, string? unit, string? accuracy, string? supplyVoltage, string? outputSignal, string? ingressProtection, string? connectorType, string? processConnection, string? responseTime, string? otherSpecifications, string? installationLocation, string? measuredMedium, string? operatingTemperature, string? operatingPressure, string? calibrationFrequency, string? failureHistory, string? environmentCondition, ModuleInformationModel? moduleInformationModel, List<JBInformationModel>? jbInformationModels, List<ImageInformationModel>? additionalConnectionImages)
    {
        Code = code;
        Function = function;
        Type = type;
        ModelSeries = modelSeries;
        Manufacturer = manufacturer;
        PartNumber = partNumber;
        SerialNumber = serialNumber;
        ManufacturingYear = manufacturingYear;
        InstallationDate = installationDate;
        IOAddress = ioAddress;
        MeasurementType = measurementType;
        Range = range;
        Unit = unit;
        Accuracy = accuracy;
        SupplyVoltage = supplyVoltage;
        OutputSignal = outputSignal;
        IngressProtection = ingressProtection;
        ConnectorType = connectorType;
        ProcessConnection = processConnection;
        ResponseTime = responseTime;
        OtherSpecifications = otherSpecifications;
        InstallationLocation = installationLocation;
        MeasuredMedium = measuredMedium;
        OperatingTemperature = operatingTemperature;
        OperatingPressure = operatingPressure;
        CalibrationFrequency = calibrationFrequency;
        FailureHistory = failureHistory;
        EnvironmentCondition = environmentCondition;
        ModuleInformationModel = moduleInformationModel;
        JBInformationModels = jbInformationModels;
        AdditionalConnectionImages = additionalConnectionImages;
    }
  public DeviceInformationModel(int id, string code)
  {
    Id = id;
    Code = code;
  }

  public DeviceInformationModel()
  {
  }
}


[Preserve]
public class DeviceGeneralModel
{
  [JsonProperty("id")]
  public string? Id { get; set; }

  [JsonProperty("code")]
  public string? Code { get; set; }

    // Basic Information
    [JsonProperty("function")] public string Function { get; set; }
    [JsonProperty("ioAddress")] public string IOAddress { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("modelSeries")] public string ModelSeries { get; set; }
    [JsonProperty("manufacturer")] public string Manufacturer { get; set; }
    [JsonProperty("partNumber")] public string PartNumber { get; set; }
    [JsonProperty("serialNumber")] public string SerialNumber { get; set; }
    [JsonProperty("manufacturingYear")] public string ManufacturingYear { get; set; }
    [JsonProperty("installationDate")] public string InstallationDate { get; set; }

    // Technical Information
    [JsonProperty("measurementType")] public string MeasurementType { get; set; }
    [JsonProperty("range")] public string Range { get; set; }
    [JsonProperty("unit")] public string Unit { get; set; }
    [JsonProperty("accuracy")] public string Accuracy { get; set; }
    [JsonProperty("supplyVoltage")] public string SupplyVoltage { get; set; }
    [JsonProperty("outputSignal")] public string OutputSignal { get; set; }
    [JsonProperty("ingressProtection")] public string IngressProtection { get; set; }
    [JsonProperty("connectorType")] public string ConnectorType { get; set; }
    [JsonProperty("processConnection")] public string ProcessConnection { get; set; }
    [JsonProperty("responseTime")] public string ResponseTime { get; set; }
    [JsonProperty("otherSpecifications")] public string OtherSpecifications { get; set; }

    // Operational Information
    [JsonProperty("installationLocation")] public string InstallationLocation { get; set; }
    [JsonProperty("measuredMedium")] public string MeasuredMedium { get; set; }
    [JsonProperty("operatingTemperature")] public string OperatingTemperature { get; set; }
    [JsonProperty("operatingPressure")] public string OperatingPressure { get; set; }
    [JsonProperty("calibrationFrequency")] public string CalibrationFrequency { get; set; }
    [JsonProperty("failureHistory")] public string FailureHistory { get; set; }
    [JsonProperty("environmentCondition")] public string EnvironmentCondition { get; set; }

    // Additional Information
    [JsonProperty("module")]
  public ModuleBasicModel? ModuleBasicModel { get; set; }

  [JsonProperty("JB")]
  public JBBasicModel JBBasicModel { get; set; }

  [JsonProperty("additionalConnectionImages")]
  public List<ImageBasicModel> AdditionalImageModels { get; set; }

  [Preserve]

    public DeviceGeneralModel(string? id, string? code, string? function, string? type, string? modelSeries, string? manufacturer, string? partNumber, string? serialNumber, string? manufacturingYear, string? installationDate, string? ioAddress, string measurementType, string? range, string? unit, string? accuracy, string? supplyVoltage, string? outputSignal, string? ingressProtection, string? connectorType, string? processConnection, string? responseTime, string? otherSpecifications, string? installationLocation, string? measuredMedium, string? operatingTemperature, string? operatingPressure, string? calibrationFrequency, string? failureHistory, string? environmentCondition, ModuleBasicModel? moduleBasicModel, JBBasicModel jbBasicModels, List<ImageBasicModel>? additionalImageModels)
    {
        Id = id;
        Code = code;
        Function = function;
        Type = type;
        ModelSeries = modelSeries;
        Manufacturer = manufacturer;
        PartNumber = partNumber;
        SerialNumber = serialNumber;
        ManufacturingYear = manufacturingYear;
        InstallationDate = installationDate;
        IOAddress = ioAddress;
        MeasurementType = measurementType;
        Range = range;
        Unit = unit;
        Accuracy = accuracy;
        SupplyVoltage = supplyVoltage;
        OutputSignal = outputSignal;
        IngressProtection = ingressProtection;
        ConnectorType = connectorType;
        ProcessConnection = processConnection;
        ResponseTime = responseTime;
        OtherSpecifications = otherSpecifications;
        InstallationLocation = installationLocation;
        MeasuredMedium = measuredMedium;
        OperatingTemperature = operatingTemperature;
        OperatingPressure = operatingPressure;
        CalibrationFrequency = calibrationFrequency;
        FailureHistory = failureHistory;
        EnvironmentCondition = environmentCondition;
        ModuleBasicModel = moduleBasicModel;
        JBBasicModel = jbBasicModels;
        AdditionalImageModels = additionalImageModels;

    }
}


[Preserve]
public class DevicePostGeneralModel
{
  [JsonProperty("code")]
  public string? Code { get; set; }
    // Basic Information
    [JsonProperty("function")] public string Function { get; set; }
    [JsonProperty("ioAddress")] public string IOAddress { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("modelSeries")] public string ModelSeries { get; set; }
    [JsonProperty("manufacturer")] public string Manufacturer { get; set; }
    [JsonProperty("partNumber")] public string PartNumber { get; set; }
    [JsonProperty("serialNumber")] public string SerialNumber { get; set; }
    [JsonProperty("manufacturingYear")] public string ManufacturingYear { get; set; }
    [JsonProperty("installationDate")] public string InstallationDate { get; set; }

    // Technical Information
    [JsonProperty("measurementType")] public string MeasurementType { get; set; }
    [JsonProperty("range")] public string Range { get; set; }
    [JsonProperty("unit")] public string Unit { get; set; }
    [JsonProperty("accuracy")] public string Accuracy { get; set; }
    [JsonProperty("supplyVoltage")] public string SupplyVoltage { get; set; }
    [JsonProperty("outputSignal")] public string OutputSignal { get; set; }
    [JsonProperty("ingressProtection")] public string IngressProtection { get; set; }
    [JsonProperty("connectorType")] public string ConnectorType { get; set; }
    [JsonProperty("processConnection")] public string ProcessConnection { get; set; }
    [JsonProperty("responseTime")] public string ResponseTime { get; set; }
    [JsonProperty("otherSpecifications")] public string OtherSpecifications { get; set; }

    // Operational Information
    [JsonProperty("installationLocation")] public string InstallationLocation { get; set; }
    [JsonProperty("measuredMedium")] public string MeasuredMedium { get; set; }
    [JsonProperty("operatingTemperature")] public string OperatingTemperature { get; set; }
    [JsonProperty("operatingPressure")] public string OperatingPressure { get; set; }
    [JsonProperty("calibrationFrequency")] public string CalibrationFrequency { get; set; }
    [JsonProperty("failureHistory")] public string FailureHistory { get; set; }
    [JsonProperty("environmentCondition")] public string EnvironmentCondition { get; set; }

    // Additional Information
    [JsonProperty("module")]
  public ModuleBasicModel? ModuleBasicModel { get; set; }

  [JsonProperty("JB")]
  public JBBasicModel JBBasicModel { get; set; }

  [JsonProperty("additionalConnectionImages")]
  public List<ImageBasicModel> AdditionalConnectionBasicModel { get; set; }

  [Preserve]

    public DevicePostGeneralModel(string? code, string? function, string? type, string? modelSeries, string? manufacturer, string? partNumber, string? serialNumber, string? manufacturingYear, string? installationDate, string? ioAddress, string measurementType, string? range, string? unit, string? accuracy, string? supplyVoltage, string? outputSignal, string? ingressProtection, string? connectorType, string? processConnection, string? responseTime, string? otherSpecifications, string? installationLocation, string? measuredMedium, string? operatingTemperature, string? operatingPressure, string? calibrationFrequency, string? failureHistory, string? environmentCondition, ModuleBasicModel? moduleBasicModel, JBBasicModel jbBasicModels, List<ImageBasicModel>? additionalImageModel)
    {
        Code = code;
        Function = function;
        Type = type;
        ModelSeries = modelSeries;
        Manufacturer = manufacturer;
        PartNumber = partNumber;
        SerialNumber = serialNumber;
        ManufacturingYear = manufacturingYear;
        InstallationDate = installationDate;
        IOAddress = ioAddress;
        MeasurementType = measurementType;
        Range = range;
        Unit = unit;
        Accuracy = accuracy;
        SupplyVoltage = supplyVoltage;
        OutputSignal = outputSignal;
        IngressProtection = ingressProtection;
        ConnectorType = connectorType;
        ProcessConnection = processConnection;
        ResponseTime = responseTime;
        OtherSpecifications = otherSpecifications;
        InstallationLocation = installationLocation;
        MeasuredMedium = measuredMedium;
        OperatingTemperature = operatingTemperature;
        OperatingPressure = operatingPressure;
        CalibrationFrequency = calibrationFrequency;
        FailureHistory = failureHistory;
        EnvironmentCondition = environmentCondition;
        ModuleBasicModel = moduleBasicModel;
        JBBasicModel = jbBasicModels;
        AdditionalConnectionBasicModel = additionalImageModel;
    }
}


[Preserve]
public class DeviceBasicModel
{
  [JsonProperty("id")]
  public string? Id { get; set; }
  [JsonProperty("code")]
  public string? Code { get; set; }
  [Preserve]

  public DeviceBasicModel(string? id, string? code)
  {
    Id = id;
    Code = code;
  }

}