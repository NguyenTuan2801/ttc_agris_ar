using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Domain.Interfaces;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Purchasing;


namespace Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly HttpClient _httpClient;


        public ImageRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            // _httpClient.BaseAddress = new Uri(GlobalVariable.baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        //! TRả về ImageEntity
        public async Task<ImageEntity> GetImageByIdAsync(int ImageId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Images/{ImageId}/info");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to get Image. Status: {response.StatusCode}");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    //Debug.Log(content);
                    var entity = JsonConvert.DeserializeObject<ImageEntity>(content);
                    return entity;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Image", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }

        }

        //! Trả về List<ImageEntity>
        public async Task<List<ImageEntity>> GetListImageAsync(int grapperId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Grappers/{grapperId}/images");
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to get Image list. Status: {response.StatusCode}");
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    //Debug.Log(content);
                    var entities = JsonConvert.DeserializeObject<List<ImageEntity>>(content);
                    return entities;
                }

            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Image list", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }


        public async Task<bool> CreateNewImageAsync(int grapperId, ImageEntity ImageEntity)
        {
            try
            {
                if (ImageEntity == null)
                    throw new ArgumentNullException(nameof(ImageEntity), "Request data cannot be null");

                // var json = JsonConvert.SerializeObject(ImageEntity);
                // Tạo dữ liệu tối giản gửi lên server với tên property khớp yêu cầu
                // var ImageImageEntity = ConvertImageImageEntity(ImageEntity);
                var json = JsonConvert.SerializeObject(ImageEntity);

                //Debug.Log(json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{GlobalVariable.baseUrl}/Images/{grapperId}?fileName={ImageEntity.Name}", content);

                var temp = await response.Content.ReadAsStringAsync();
                //Debug.Log(temp);
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to create Image", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> DeleteImageAsync(int ImageId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{GlobalVariable.baseUrl}/Images/{ImageId}");
                var temp = await response.Content.ReadAsStringAsync();
                //Debug.Log(temp);
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to delete Image", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> UploadNewImageFromGallery(int grapperId, Texture2D texture, string fileName, string filePath, string mimeType)
        {
            try
            {
                // Chuẩn hóa file name
                fileName = Path.GetFileNameWithoutExtension(fileName) + ".png";

                if (texture.height == 0)
                    throw new Exception("Texture height is zero!");

                // Resize ảnh dùng GPU (Graphics.Blit)
                const int targetHeight = 2560;
                float aspectRatio = (float)texture.width / texture.height;
                int targetWidth = Mathf.Max(1, Mathf.RoundToInt(targetHeight * aspectRatio));

                RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
                Graphics.Blit(texture, rt);

                Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
                RenderTexture.active = rt;
                resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
                resizedTexture.Apply();
                RenderTexture.active = null;
                RenderTexture.ReleaseTemporary(rt);

                // Encode resized image
                byte[] imageData = resizedTexture.EncodeToPNG();
                UnityEngine.Object.Destroy(resizedTexture); // Cleanup

                // Chuẩn bị form upload
                WWWForm form = new WWWForm();
                form.AddBinaryData("file", imageData, fileName, "image/png");
                Debug.Log($"UploadNewImageFromGallery: {grapperId} + {fileName}");
                // Upload
                using (UnityWebRequest request = UnityWebRequest.Post($"{GlobalVariable.baseUrl}/Images/{grapperId}?fileName={fileName}", form))
                {
                    var operation = request.SendWebRequest();
                    while (!operation.isDone)
                    {
                        await Task.Yield();
                    }

                    if (request.result != UnityWebRequest.Result.Success)
                        throw new Exception($"Lỗi upload ảnh: {request.error}");

                    string responseText = request.downloadHandler.text;
                    return JsonConvert.DeserializeObject<bool>(responseText);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to upload image", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Lỗi upload ảnh: {ex.Message}", ex);
            }
        }
        public async Task<bool> UploadNewImageFromCamera(int grapperId, Texture2D texture, string fileName)
        {
            try
            {
                // Đảm bảo đuôi .png
                fileName = Path.GetFileNameWithoutExtension(fileName) + ".png";

                if (texture.height == 0)
                    throw new Exception("Texture height is zero!");

                // Resize ảnh (dùng GPU - nhanh hơn nhiều)
                const int targetHeight = 2560;
                float aspectRatio = (float)texture.width / texture.height;
                int targetWidth = Mathf.Max(1, Mathf.RoundToInt(targetHeight * aspectRatio));

                RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
                Graphics.Blit(texture, rt);

                Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
                RenderTexture.active = rt;
                resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
                resizedTexture.Apply();
                RenderTexture.active = null;
                RenderTexture.ReleaseTemporary(rt);

                // Encode ảnh
                byte[] imageData = resizedTexture.EncodeToPNG();
                UnityEngine.Object.Destroy(resizedTexture);

                // Form dữ liệu
                WWWForm form = new WWWForm();
                form.AddBinaryData("file", imageData, fileName, "image/png");

                using (UnityWebRequest request = UnityWebRequest.Post(
                    $"{GlobalVariable.baseUrl}/Images/{grapperId}?fileName={fileName}", form))
                {
                    var operation = request.SendWebRequest();
                    while (!operation.isDone)
                    {
                        await Task.Yield();
                    }

                    if (request.result != UnityWebRequest.Result.Success)
                        throw new Exception($"Lỗi upload ảnh: {request.error}");

                    string responseText = request.downloadHandler.text;
                    return JsonConvert.DeserializeObject<bool>(responseText);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to upload image", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex);
            }
        }

        // public IEnumerator UploadNewImage()
        // {
        //     // Chụp ảnh màn hình
        //     string filePath = Path.Combine(Application.persistentDataPath, "screenshot.png");

        //     yield return new WaitForEndOfFrame();
        //     // Đọc dữ liệu ảnh
        //     byte[] imageBytes = File.ReadAllBytes(filePath);

        //     // Tạo DTO
        //     ImageDto imageDto = new ImageDto
        //     {
        //         FileName = "screenshot.png",
        //         ContentType = "image/png",
        //         Size = imageBytes.Length,
        //         Description = "Ảnh chụp từ Unity"
        //     };

        //     // Chuyển DTO thành JSON
        //     string jsonDto = JsonUtility.ToJson(imageDto);

        //     // Tạo form để gửi cả file và DTO
        //     WWWForm form = new WWWForm();
        //     form.AddBinaryData("file", imageBytes, "screenshot.png", "image/png");

        //     form.AddField("metadata", jsonDto);

        //     using (UnityWebRequest www = UnityWebRequest.Post(GlobalVariable.baseUrl, form))
        //     {
        //         yield return www.SendWebRequest();

        //         if (www.result == UnityWebRequest.Result.Success)
        //         {
        //         }
        //         else
        //         {
        //         }
        //     }

        //     // Xóa file tạm
        //     File.Delete(filePath);
        // }
        // private object ConvertImageRequestData(ImageEntity ImageEntity)
        // {
        //     return new
        //     {
        //         name = ImageEntity.Name,
        //         Location = ImageEntity.Location ?? "",
        //         ListDevices = ImageEntity.DeviceEntities?
        //             .Where(d => d != null)
        //             .Select(d => new DeviceBasicDto(d.Id, d.Code))
        //             .ToList() ?? new List<DeviceBasicDto>(),
        //         ListModules = ImageEntity.ModuleEntities
        //             .Where(m => m != null)
        //             .Select(m => new ModuleBasicDto(m.Id, m.Name))
        //             .ToList() ?? new List<ModuleBasicDto>(),
        //         OutdoorImage = ImageEntity.OutdoorImageEntity != null
        //             ? new ImageBasicDto(ImageEntity.OutdoorImageEntity.Id, ImageEntity.OutdoorImageEntity.Name)
        //             : null,
        //         ListConnectionImages = ImageEntity.ConnectionImageEntities?
        //             .Where(i => i != null)
        //             .Select(i => new ImageBasicDto(i.Id, i.Name))
        //             .ToList() ?? new List<ImageBasicDto>()
        //     };
        // }



    }
}