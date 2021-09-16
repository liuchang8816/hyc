// 下列 ifdef 块是创建使从 DLL 导出更简单的
// 宏的标准方法。此 DLL 中的所有文件都是用命令行上定义的 CAMERA2D_EXPORTS
// 符号编译的。在使用此 DLL 的
// 任何项目上不应定义此符号。这样，源文件中包含此文件的任何其他项目都会将
// CAMERA2D_API 函数视为是从 DLL 导入的，而此 DLL 则将用此宏定义的
// 符号视为是被导出的。
#ifdef CAMERA2D_EXPORTS
#define CAMERA2D_API __declspec(dllexport)
#else
#define CAMERA2D_API __declspec(dllimport)
#endif

extern "C"
{
    CAMERA2D_API bool Cam_Search(char *pszDesc);

    CAMERA2D_API bool Cam_Open();

    CAMERA2D_API bool Cam_Close();

    CAMERA2D_API bool Cam_StartAcquire();

    CAMERA2D_API bool Cam_StopAcquire();

    CAMERA2D_API bool Cam_GetExposureTime(INT64 *time);

    CAMERA2D_API bool Cam_SetExposureTime(INT64 time);

    CAMERA2D_API bool Cam_GetImageSize(int *nWidth, int *nHeight);

    CAMERA2D_API bool Cam_GetImage(unsigned char *pData);

    CAMERA2D_API bool Cam_FreeImage();
}

