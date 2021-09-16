#ifndef VOMMACAMERA_H
#define VOMMACAMERA_H

#include <VOMMACam3D.h>

#ifdef VOMMACAMERA_EXPORTS
#define VOMMACAMERAAPI __declspec(dllexport)
#else
#define VOMMACAMERAAPI __declspec(dllimport)
#endif

typedef void(__stdcall* CameraEventCallBack)(uchar* data, size_t width, size_t height);

#ifdef __cplusplus
extern "C" {
#endif

	VOMMACAMERAAPI bool CameraSearch(char* desc);
	VOMMACAMERAAPI bool CameraOpen();
	VOMMACAMERAAPI bool CameraClose();
	VOMMACAMERAAPI bool CameraSetTriggerMode(int mode); //0-freerun 1-softtrigger
	VOMMACAMERAAPI bool CameraSoftTrigger();
	VOMMACAMERAAPI bool CameraStartAcquisition();
	VOMMACAMERAAPI bool CameraStopAcquisition();
	VOMMACAMERAAPI bool CameraGetExposureTime(int64_t& time);
	VOMMACAMERAAPI bool CameraSetExposureTime(int64_t time);
	VOMMACAMERAAPI bool CameraGetImageSize(size_t& width, size_t& height);
	VOMMACAMERAAPI bool CameraGetImage(uchar* data);
	VOMMACAMERAAPI void CameraSetDepthCalPara(Variables vars);
	VOMMACAMERAAPI bool CameraGetDepthImg(uchar* data, size_t width, size_t height, uchar* depthData, const char* depthDir);

	VOMMACAMERAAPI void RegisterImageReceived(CameraEventCallBack callBack);

#ifdef __cplusplus
}
#endif

#endif

