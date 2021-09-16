// Camera3D.cpp : 定义 DLL 的导出函数。
//

// Camera2D.cpp : 定义 DLL 的导出函数。
//

#include "pch.h"
#include "framework.h"
#include "Camera3D.h"
#include <stdio.h>

#include <fstream>
#include <iostream>
#include <sstream>

using namespace std;

bool g_bOpened = false;
bool g_bStartAcquire = false;
INT64 g_ETime = 12048;
string g_strDepthPath;
string g_strCenterPath;


extern "C"
{
    CAMERA3D_API bool Cam_Search(char *pszDesc)
    {
        strcpy_s(pszDesc, 64, "Virtual 3D Cammera#0");
        return true;
    }

    CAMERA3D_API bool Cam_Open()
    {
        if (g_bOpened)
        {
            return true;
        }

        g_bOpened = true;

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

        g_bOpened = false;

        return true;
    }

    CAMERA3D_API bool Cam_StartAcquire()
    {
        g_bStartAcquire = true;
        return true;
    }

    CAMERA3D_API bool Cam_StopAcquire()
    {
        g_bStartAcquire = false;
        return true;
    }

    CAMERA3D_API bool Cam_GetExposureTime(INT64 *time)
    {
        *time = g_ETime;
        return true;
    }

    CAMERA3D_API bool Cam_SetExposureTime(INT64 time)
    {
        g_ETime = time;
        return true;
    }

    CAMERA3D_API bool Cam_GetImageSize(int *nWidth, int *nHeight, int *nBitDepth)
    {
        *nHeight = 768;
        *nWidth = 1024;
        *nBitDepth = 8;
        return true;
    }

    CAMERA3D_API bool Cam_GetImage(unsigned char *pData)
    {
        string strImgData;
        ifstream  ifsImg;

        ifsImg.exceptions(std::ifstream::failbit | std::ifstream::badbit);

        try
        {
            ifsImg.open(".\\testimg.bmp");
            stringstream ssImg;
            ssImg << ifsImg.rdbuf();
            ifsImg.close();
            strImgData = ssImg.str();
        }
        catch (std::ifstream::failure e)
        {
            return false;
        }

        const char* pImgData = strImgData.c_str();
        memcpy_s(pData, 1024 * 768, pImgData + 1078, 1024 * 768);
        return true;
    }
}