// Camera3D.cpp : 定义 DLL 的导出函数。
//

#include "pch.h"
#include "framework.h"
#include "Camera3D.h"
#include "VOMMACam3D.h"
#include <stdio.h>


_Descriptor g_CamDesc = { 0 };
bool g_bOpened = false;
bool g_bStartAcquire = false;
_VOMMAImage g_Image = { 0 };
_Variables g_Vars;
string g_strDepthPath;
string g_strCenterPath;

int GetImageBitDepth(int nImgType);

int GetImageBitDepth(int nImgType)
{
    int nBitDepth = 0;
    switch (g_Image.imageType)
    {
    case VOMMA_8UC1:
    case VOMMA_8UC3:
        nBitDepth = 8;
        break;

    case VOMMA_16UC1:
    case VOMMA_16UC3:
        nBitDepth = 16;
        break;

    case VOMMA_32FC1:
    case VOMMA_32FC3:
        nBitDepth = 32;
        break;

    default:
        nBitDepth = 8;
        break;
    }

    return nBitDepth;
}

extern "C"
{
    CAMERA3D_API bool Cam_Search(char *pszDesc)
    {
        bool bFound = false;
        int nDevNums = 0;
        _Descriptor aryDesc[20] = { 0 };
        VOMMAEnumDevices(&nDevNums);

        for (int y = 0; y < nDevNums; y++)
        {
            VOMMADeviceDescriptor(&aryDesc[y], y);
            string strName = aryDesc[y].name;
            string name = "EURESYS#Euresys Grablink Full";
            if (strName.find(name) != strName.npos)
            {
                bFound = true;
                g_CamDesc = aryDesc[y];
                break;
            }
        }

        if (bFound)
        {
            strcpy_s(pszDesc, 64, g_CamDesc.name);
            return true;
        }

        return false;
    }

    CAMERA3D_API bool Cam_Open()
    {
        if (g_bOpened)
        {
            return true;
        }
        OutputDebugString("Cam_Open()");
        OutputDebugString(g_CamDesc.name);
        if (S_NO_ERROR != VOMMADeviceInit(ExclusiveAccess, CL_EURESYS, "EURESYS#Euresys Grablink Full#0"))
        {
            return false;
        }
        VOMMASetPixelFormat(0, g_CamDesc.name);
        VOMMASetExposureTime(12048, g_CamDesc.name);

        if (S_NO_ERROR != VOMMAPreProcess(".\\wb.txt"))
        {
            VOMMADeviceExit(g_CamDesc.name);
            return false;
        }

        g_bOpened = true;

        // TODO: GET INI CONFIGS

        return true;
    }

    CAMERA3D_API bool Cam_Close()
    {
        if (g_bStartAcquire)
        {
            if (!Cam_StopAcquire())
            {
                return false;
            }
        }

        g_bStartAcquire = false;

        if (S_NO_ERROR != VOMMADeviceExit(g_CamDesc.name))
        {
            return false;
        }

        g_bOpened = false;

        return true;
    }

    CAMERA3D_API bool Cam_StartAcquire()
    {
        return (S_NO_ERROR == VOMMAOpenStreamChannel(g_CamDesc.name));
    }

    CAMERA3D_API bool Cam_StopAcquire()
    {
        return (S_NO_ERROR == VOMMACloseStreamChannel(g_CamDesc.name));
    }

    CAMERA3D_API bool Cam_GetExposureTime(INT64 *time)
    {
        return (S_NO_ERROR == VOMMAGetExposureTime(time, g_CamDesc.name));
    }

    CAMERA3D_API bool Cam_SetExposureTime(INT64 time)
    {
        return (S_NO_ERROR == VOMMASetExposureTime(time, g_CamDesc.name));
    }

    CAMERA3D_API bool Cam_GetImageSize(int *nWidth, int *nHeight, int *nBitDepth)
    {
        if (S_NO_ERROR == VOMMAGetFrame(g_Image, g_CamDesc.name))
        {
            char szMsg[1024] = { 0 };
            sprintf_s(szMsg, "width:%d, height:%d, imagetype:%d", g_Image.cols, g_Image.rows, g_Image.imageType);
            OutputDebugString(szMsg);
            *nHeight = g_Image.rows;
            *nWidth = g_Image.cols;
            *nBitDepth = GetImageBitDepth(g_Image.imageType);
            return true;
        }
        return false;
    }

    CAMERA3D_API bool Cam_GetImage(unsigned char *pData)
    {
        if (S_NO_ERROR != VOMMAGetDepthImg(g_Vars, g_Image, g_strDepthPath, g_strCenterPath))
        {
            return false;
        }

        memcpy_s(pData, g_Image.cols * g_Image.rows * GetImageBitDepth(g_Image.imageType)/8,
            g_Image.imageData, g_Image.rows * GetImageBitDepth(g_Image.imageType)/8);

        return true;
    }

}