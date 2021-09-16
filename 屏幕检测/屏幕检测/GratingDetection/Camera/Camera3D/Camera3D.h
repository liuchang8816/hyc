// 下列 ifdef 块是创建使从 DLL 导出更简单的
// 宏的标准方法。此 DLL 中的所有文件都是用命令行上定义的 CAMERA3D_EXPORTS
// 符号编译的。在使用此 DLL 的
// 任何项目上不应定义此符号。这样，源文件中包含此文件的任何其他项目都会将
// CAMERA3D_API 函数视为是从 DLL 导入的，而此 DLL 则将用此宏定义的
// 符号视为是被导出的。
#ifdef CAMERA3D_EXPORTS
#define CAMERA3D_API __declspec(dllexport)
#else
#define CAMERA3D_API __declspec(dllimport)
#endif

extern "C"
{
    CAMERA3D_API bool Cam_Search(char *pszDesc);

    CAMERA3D_API bool Cam_Open();

    CAMERA3D_API bool Cam_Close();

    CAMERA3D_API bool Cam_StartAcquire();

    CAMERA3D_API bool Cam_StopAcquire();

    CAMERA3D_API bool Cam_GetExposureTime(INT64 *time);

    CAMERA3D_API bool Cam_SetExposureTime(INT64 time);

    CAMERA3D_API bool Cam_GetImageSize(int *nWidth, int *nHeight, int *nBitDepth);

    CAMERA3D_API bool Cam_GetImage(unsigned char *pData);
}
