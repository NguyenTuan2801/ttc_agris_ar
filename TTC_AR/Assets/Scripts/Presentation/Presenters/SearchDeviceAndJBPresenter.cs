using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Interfaces;
using Unity.VisualScripting;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Rack;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
public class SearchDeviceAndJBPresenter
{
    private readonly ISearchDeviceAndJBView _view;
    private readonly IJBService _JBService;
    private readonly IDeviceService _DeviceService;

    public SearchDeviceAndJBPresenter(
        ISearchDeviceAndJBView view,
        IJBService jBService,
        IDeviceService deviceService)
    {
        _view = view;
        _JBService = jBService;
        _DeviceService = deviceService;
    }

    //! Get list SearchDeviceAndJB chỉ có Id và Code
    public async void LoadDataForSearching(int grapperId)
    {
        GlobalVariable.APIRequestType.Add("GET_JB_List_Information");
        GlobalVariable.APIRequestType.Add("GET_Device_List_Information_FromGrapper");
        _view.ShowLoading("Đang tải dữ liệu...");

        try
        {
            var jBGeneralDtosTask = _JBService.GetListJBInformationAsync(grapperId);
            var deviceResponseDtosTask = _DeviceService.GetListDeviceInformationFromGrapperAsync(grapperId);

            await Task.WhenAll(jBGeneralDtosTask, deviceResponseDtosTask);

            // XỬ LÝ JB
            if (jBGeneralDtosTask.Result != null)
            {
                var models = jBGeneralDtosTask.Result
                    .Select(dto => ConvertJBFromGeneralDto(dto))
                    .ToList();

                GlobalVariable_Search_Devices.temp_ListJBInformationModel = models;
                GlobalVariable_Search_Devices.temp_Dictionary_JBInformationModel = models
                    .ToDictionary(jb => jb.Name, jb => jb);
            }

            // XỬ LÝ DEVICE
            if (deviceResponseDtosTask.Result != null)
            {
                var models = deviceResponseDtosTask.Result
                    .Select(dto => ConvertDeviceFromResponseDto(dto))
                    .ToList();

                GlobalVariable_Search_Devices.all_Device_Models = new List<DeviceInformationModel>(models);   // ← Quan trọng
                GlobalVariable_Search_Devices.temp_ListDeviceInformationModel = new List<DeviceInformationModel>(models);
            }

            _view.ShowSuccess();
            _view.SetInit();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in LoadDataForSearching: {ex.Message}");
            _view.ShowError("Tải dữ liệu thất bại");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_JB_List_Information");
            GlobalVariable.APIRequestType.Remove("GET_Device_List_Information_FromGrapper");
        }
    }
    public async Task LoadImageAsync(string name, Image imagePrefab)
    {
        await LoadImage.Instance.LoadImageFromUrlAsync(name, imagePrefab);
    }

    private JBInformationModel ConvertJBFromGeneralDto(JBGeneralDto dto)
    {
        return new JBInformationModel(
            id: dto.Id,
            name: dto.Name,
            location: string.IsNullOrEmpty(dto.Location) ? "Được ghi chú trên sơ đồ" : dto.Location,
            outdoorImage: dto.OutdoorImageBasicDto != null ? new ImageInformationModel(
                 id: dto.OutdoorImageBasicDto.Id,
               name: dto.OutdoorImageBasicDto.Name
            ) : null,

            listConnectionImages: dto.ConnectionImageBasicDtos.Any() ? dto.ConnectionImageBasicDtos.Select(imageDto => new ImageInformationModel(
                id: imageDto.Id,
               name: imageDto.Name
            )).ToList() : new List<ImageInformationModel>()

        );
    }
    //! Dto => Model
    private DeviceInformationModel ConvertDeviceFromResponseDto(DeviceResponseDto dto)
    {
        return new DeviceInformationModel(
            id: dto.Id,
            code: dto.Code,
            function: dto.Function,
            type: dto.Type,
            ioAddress: dto.IOAddress,
            modelSeries: dto.ModelSeries,
            manufacturer: dto.Manufacturer,
            partNumber: dto.PartNumber,
            serialNumber: dto.SerialNumber,
            manufacturingYear: dto.ManufacturingYear,
            installationDate: dto.InstallationDate,            
            measurementType: dto.MeasurementType,
            range: dto.Range,
            unit: dto.Unit,
            accuracy: dto.Accuracy,
            testError: dto.TestError,
            lengthOrDN: dto.LengthOrDN,
            supplyVoltage: dto.SupplyVoltage,
            outputSignal: dto.OutputSignal,
            ingressProtection: dto.IngressProtection,
            connectorType: dto.ConnectorType,
            processConnection: dto.ProcessConnection,
            responseTime: dto.ResponseTime,
            otherSpecifications: dto.OtherSpecifications,
            jbInformationModels: dto.JBBasicDtos.Any() ? dto.JBBasicDtos.Select(jb => new JBInformationModel(
                id: jb.Id,
                name: jb.Name
            )).ToList() : new List<JBInformationModel>(),
            moduleInformationModel: dto.ModuleBasicDto != null ? new ModuleInformationModel(
                                        dto.ModuleBasicDto.Id,
                                        dto.ModuleBasicDto.Name
            ) : null,
            additionalConnectionImages: dto.AdditionalImageBasicDtos.Any() ? dto.AdditionalImageBasicDtos?.Select(
                imageDto => new ImageInformationModel(
                                id: imageDto.Id,
                                name: imageDto.Name
                // ,
                // url: imageDto.Url
                )).ToList() : new List<ImageInformationModel>()
            );
    }
}
