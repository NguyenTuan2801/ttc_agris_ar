using System;
using System.Collections.Generic;
using ApplicationLayer.Dtos.Image;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.Module;
using Newtonsoft.Json;
using UnityEngine.Scripting;

#nullable enable

namespace ApplicationLayer.Dtos.Device
{
    [Preserve]
    public class DeviceRequestDto
    {
        [JsonProperty("code")] public string Code { get; set; }

        // Basic Information
        [JsonProperty("function")] public string Function { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("ioAddress")] public string IOAddress { get; set; }       
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
        [JsonProperty("testError")] public string TestError { get; set; }
        [JsonProperty("lengthOrDN")] public string LengthOrDN { get; set; }
        [JsonProperty("supplyVoltage")] public string SupplyVoltage { get; set; }
        [JsonProperty("outputSignal")] public string OutputSignal { get; set; }
        [JsonProperty("ingressProtection")] public string IngressProtection { get; set; }
        [JsonProperty("connectorType")] public string ConnectorType { get; set; }
        [JsonProperty("processConnection")] public string ProcessConnection { get; set; }
        [JsonProperty("responseTime")] public string ResponseTime { get; set; }
        [JsonProperty("otherSpecifications")] public string OtherSpecifications { get; set; }
       
        // Additional Information
        [JsonProperty("module")] public ModuleBasicDto? ModuleBasicDto { get; set; }
        [JsonProperty("jBs")] public List<JBBasicDto>? JBBasicDtos { get; set; }
        [JsonProperty("additionalConnectionImages")] public List<ImageBasicDto>? AdditionalImageBasicDtos { get; set; }

        [Preserve]


        public DeviceRequestDto(string code, string function, string ioAddress, string type, string modelSeries, string manufacturer, string partNumber, string serialNumber, string manufacturingYear, string installationDate, string measurementType, string range, string unit, string accuracy, string testError, string lengthOrDN, string supplyVoltage, string outputSignal, string ingressProtection, string connectorType, string processConnection, string responseTime, string otherSpecifications, ModuleBasicDto? moduleBasicDto, List<JBBasicDto>? jbBasicDtos, List<ImageBasicDto>? additionalImageBasicDtos)
        {
            Code = string.IsNullOrEmpty(code) ? throw new ArgumentException(nameof(code)) : code;
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
            ModuleBasicDto = moduleBasicDto;
            JBBasicDtos = jbBasicDtos;
            AdditionalImageBasicDtos = additionalImageBasicDtos;

            // [Preserve]
            // 
            // public DeviceRequestDto(string code, string function, string range, string unit, string ioAddress, ModuleBasicDto moduleBasicDto, JBBasicDto jbBasicDto, List<ImageBasicDto> additionalImageBasicDtos)
            // {
            //     Code = string.IsNullOrEmpty(code) ? throw new System.ArgumentException(nameof(code)) : code;
            //     Function = function == "" ? string.Empty : function;
            //     Range = range == "" ? string.Empty : range;
            //     Unit = unit == "" ? string.Empty : unit;
            //     IOAddress = ioAddress == "" ? string.Empty : ioAddress;
            //     ModuleBasicDto = moduleBasicDto ?? throw new ArgumentException(nameof(moduleBasicDto));
            //     JBBasicDto = jbBasicDto ?? throw new ArgumentException(nameof(jbBasicDto));
            //     AdditionalImageBasicDtos = additionalImageBasicDtos ?? new List<ImageBasicDto>();
            // }
        }
    }
}