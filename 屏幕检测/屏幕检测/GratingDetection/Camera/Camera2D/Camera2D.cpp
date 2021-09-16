// Camera2D.cpp : 定义 DLL 的导出函数。
//

#include "pch.h"
#include "framework.h"
#include "Camera2D.h"
#include "VOMMACam2D.h"
#include <stdio.h>

_Descriptor g_CamDesc = { 0 };
bool g_bOpened = false;
bool g_bStartAcquire = false;
_ImgHeader g_ImgHdr = { 0 };
unsigned char *g_Data = nullptr;

extern "C"
{
    CAMERA2D_API bool Cam_Search(char *pszDesc)
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

    CAMERA2D_API bool Cam_Open()
    {
        if (g_bOpened)
        {
            return true;
        }
        OutputDebugString("Cam_Open()");
        OutputDebugString(g_CamDesc.name);
        if (S_NO_ERROR != VOMMADeviceInit(ExclusiveAccess, CL_EURESYS, g_CamDesc.name))
        {
            return false;
        }

        g_bOpened = true;

        return true;
    }

    CAMERA2D_API bool Cam_Close()
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

    CAMERA2D_API bool Cam_StartAcquire()
    {
        return (S_NO_ERROR == VOMMAOpenStreamChannel(g_CamDesc.name));
    }

    CAMERA2D_API bool Cam_StopAcquire()
    {
        return (S_NO_ERROR == VOMMACloseStreamChannel(g_CamDesc.name));
    }

    CAMERA2D_API bool Cam_GetExposureTime(INT64 *time)
    {
        return (S_NO_ERROR == VOMMAGetExposureTime(time, g_CamDesc.name));
    }

    CAMERA2D_API bool Cam_SetExposureTime(INT64 time)
    {
        return (S_NO_ERROR == VOMMASetExposureTime(time, g_CamDesc.name));
    }

    CAMERA2D_API bool Cam_GetImageSize(int *nWidth, int *nHeight)
    {
        if (S_NO_ERROR == VOMMAGetImageFromRing(&g_ImgHdr, &g_Data, g_CamDesc.name))
        {
            char szMsg[1024] = { 0 };
            sprintf_s(szMsg, "width:%d, height:%d", g_ImgHdr.width, g_ImgHdr.height);
            OutputDebugString(szMsg);
            *nHeight = g_ImgHdr.height;
            *nWidth = g_ImgHdr.width;
            return true;
        }
        return false;
    }

    CAMERA2D_API bool Cam_GetImage(unsigned char *pData)
    {
        memcpy_s(pData, g_ImgHdr.height*g_ImgHdr.width, g_Data, g_ImgHdr.height*g_ImgHdr.width);
        return true;
    }

    CAMERA2D_API bool Cam_FreeImage()
    {
        return (S_NO_ERROR == VOMMAQueueImageToRing(&g_ImgHdr, g_CamDesc.name));
    }

}