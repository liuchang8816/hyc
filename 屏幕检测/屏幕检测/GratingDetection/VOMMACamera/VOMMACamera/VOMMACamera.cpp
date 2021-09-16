#include "pch.h"
#include "VOMMACamera.h"
#include <string>

#define CALLBACK_ACQUIRE 1
#define PREALLOCATED_MEMORY 1
#define CALLBACK_DEVICEOPEN 1
#define MAX_BUFFER_NUM 3

bool bIsCamOpened = false;
std::string camDesc;
H_DEVICE hDevice;
H_STREAM hStream;
char* bufferImg[MAX_BUFFER_NUM];
std::string lasterror;

CameraEventCallBack ImageReceived;
/*接收到图像*/
void OnImageReceived(uchar* data, size_t width, size_t height)
{
	if (ImageReceived)
	{
		ImageReceived(data, width, height);
	}
}

int CallbackAcquire(H_BUFFER buffer, void* userContex)
{
	DATA_TYPE piType = 0;
	UINT64 frameid;
	size_t piSize = sizeof(UINT64);
	size_t height = 0;
	size_t width = 0;
	size_t dataSize = 0;
	char* pdata = (char*)VOMMABufferData(buffer, &dataSize);
	/* Image processing here */
	VOMMAGetBufferInfo(buffer, BUF_INFO_HEIGHT, &piType, &height, &piSize);
	VOMMAGetBufferInfo(buffer, BUF_INFO_WIDTH, &piType, &width, &piSize);

	OnImageReceived((uchar*)pdata, width, height);

	return 0;
}

bool CameraSearch(char* desc)
{
	int i, numDevs = 0;
	VOMMAEnumDevices(&numDevs);

	if (numDevs < 1)
	{
		return false;
	}

	int deviceNum = 0;
	DATA_TYPE piType;
	char* pBuffer;
	size_t piSize;
	H_FEATURE hf;

	//先获取Name的长度，再new一个空间，取到name的内容
	VOMMAGetDeviceInfo(deviceNum, DEVICE_INFO_TYPE_NAME, &piType, nullptr, &piSize);
	pBuffer = new char[piSize];
	VOMMAGetDeviceInfo(deviceNum, DEVICE_INFO_TYPE_NAME, &piType, pBuffer, &piSize);

	camDesc = std::string(pBuffer);
	strcpy_s(desc, piSize, pBuffer);
	return true;
}

bool CameraOpen()
{
	if (!bIsCamOpened)
	{
		if (H_ERR_SUCCESS == VOMMADeviceOpen(&hDevice, nullptr, nullptr, camDesc.c_str()))
		{
			string str = ".\\wb.txt";
			if (H_ERR_SUCCESS != VOMMAPreProcess(str.c_str()))
			{
				VOMMADeviceClose(hDevice);
				bIsCamOpened = false;
				lasterror = "PreProcess failed!";
				return false;
			}
			bIsCamOpened = true;
		}
	}
	return true;
}

bool CameraClose()
{
	if (bIsCamOpened)
	{
		VOMMAStreamStop(hStream);
		VOMMAStreamDelete(hStream);

#ifdef PREALLOCATED_MEMORY
		for (int i = 0; i < MAX_BUFFER_NUM; ++i)
		{
			if (bufferImg[i] != nullptr)
			{
				delete bufferImg[i];
				bufferImg[i] = nullptr;
			}
		}
#endif

		if (H_ERR_SUCCESS == VOMMADeviceClose(hDevice))
		{
			bIsCamOpened = false;
			return true;
		}
		return false;
	}
	return true;
}

bool CameraSetTriggerMode(int mode)
{
	H_FEATURE hf;
	int64_t value = 0;
	size_t piSize = sizeof(int64_t);
	if (mode == 0)  //0-freerun
	{
		if (H_ERR_SUCCESS == VOMMAGetFeature(hDevice, &hf, "TriggerMode"))
		{
			value = 0; //0 FreeMode, 1 standard
			VOMMASetPropertyInfo(hf, PROPERTY_TYPE_VALUE, &value, &piSize);
			return true;
		}
	}
	else if (mode == 1)   //1-software
	{
		int rValue = H_ERR_NONE;
		if (H_ERR_SUCCESS == VOMMAGetFeature(hDevice, &hf, "TriggerMode"))
		{
			value = 1; //0 FreeMode, 1 standard
			rValue = VOMMASetPropertyInfo(hf, PROPERTY_TYPE_VALUE, &value, &piSize);
		}
		if (rValue == H_ERR_SUCCESS && H_ERR_SUCCESS == VOMMAGetFeature(hDevice, &hf, "TriggerSource"))
		{
			value = 0;//0 software, 1 external, 2 CC1
			rValue = VOMMASetPropertyInfo(hf, PROPERTY_TYPE_VALUE, &value, &piSize);
		}
		if (rValue == H_ERR_SUCCESS)
		{
			return true;
		}
	}
	return false;
}

bool CameraSoftTrigger()
{
	H_FEATURE hf;
	int64_t value = 0;
	size_t piSize = sizeof(int64_t);
	if (H_ERR_SUCCESS == VOMMAGetFeature(hDevice, &hf, "SoftwareSignalPulse"))
	{
		value = 0;//0 triggled
		VOMMASetPropertyInfo(hf, PROPERTY_TYPE_VALUE, &value, &piSize);
		return true;
	}
	return false;
}

bool CameraStartAcquisition()
{
	if (bIsCamOpened)
	{
#ifdef CALLBACK_ACQUIRE
		hStream = VOMMAStreamNew(CallbackAcquire, nullptr, hDevice);
#else
		hStream = VOMMAStreamNew(nullptr, nullptr, hDevice);
#endif // CALLBACK_ACQUIRE
		if (hStream == nullptr) {
			lasterror = "Open stream  failed!";
			return false;
		}
		int64_t payloadSize = VOMMAGetPayloadSize(hDevice);
		for (int i = 0; i < MAX_BUFFER_NUM; ++i)
		{
#ifdef PREALLOCATED_MEMORY
			bufferImg[i] = new char[payloadSize + 1];
			if (bufferImg[i] == nullptr)
				continue;
			H_BUFFER hbuffer = VOMMABufferNew(payloadSize, bufferImg[i]);
#else
			H_BUFFER hbuffer = VOMMABufferNew(payloadSize, nullptr); // sdk malloc memory
#endif // PREALLOCATED_MEMORY
			if (nullptr != hbuffer)
			{
				VOMMABufferPush(hStream, hbuffer);
			}
			else
			{
				return false;
			}
		}
		VOMMAStreamRun(hStream);
		return true;
	}
	return false;
}

bool CameraStopAcquisition()
{
	if (hStream != nullptr)
	{
		VOMMAStreamStop(hStream);
		VOMMAStreamDelete(hStream);
	}
	return true;
}

bool CameraGetExposureTime(int64_t& time)
{
	H_FEATURE hf;
	if (H_ERR_SUCCESS == VOMMAGetFeature(hDevice, &hf, "ExposureTime"))
	{
		//如何获取"ExposureTime"的类型
		int ExposureTime = 0;
		DATA_TYPE piType;
		size_t piSize = 0;
		VOMMAGetPropertyInfo(hf, PROPERTY_TYPE_VALUE, &piType, &ExposureTime, &piSize);
		//经过上一步操作并不能获取ExposureTime的值，查看piType的值为7，查看结构体DATA_TYPE得知ExposureTime的类型为DATA_TYPE_INT64
		piSize = sizeof(int64_t);
		VOMMAGetPropertyInfo(hf, PROPERTY_TYPE_VALUE, &piType, &ExposureTime, &piSize);
		time = ExposureTime;
		return true;
	}
	return false;
}

bool CameraSetExposureTime(int64_t time)
{
	H_FEATURE hf;
	if (H_ERR_SUCCESS == VOMMAGetFeature(hDevice, &hf, "ExposureTime"))
	{
		size_t piSize = sizeof(int64_t);
		VOMMASetPropertyInfo(hf, PROPERTY_TYPE_VALUE, &time, &piSize);
		return true;
	}
	return false;
}

bool CameraGetImageSize(size_t& width, size_t& height)
{
	//获取一帧的图像数据
	DATA_TYPE piType = 0;
	H_BUFFER buffer = nullptr;
	size_t piSize = sizeof(uint64_t);
	int size = 0;
	if ((buffer = VOMMABufferPop(hStream, 3000)) != nullptr)
	{
		VOMMAGetBufferInfo(buffer, BUF_INFO_WIDTH, &piType, &width, &piSize);
		VOMMAGetBufferInfo(buffer, BUF_INFO_HEIGHT, &piType, &height, &piSize);
		VOMMABufferPush(hStream, buffer);
		return true;
	}
	return false;
}

bool CameraGetImage(uchar* data)
{
	//获取一帧的图像数据
	DATA_TYPE piType = 0;
	H_BUFFER buffer = nullptr;
	size_t piSize = sizeof(uint64_t);
	int size = 0;
	if ((buffer = VOMMABufferPop(hStream, 3000)) != nullptr)
	{
		size_t dataSize = 0;
		char* pdata = (char*)VOMMABufferData(buffer, &dataSize);
		if (pdata != nullptr)
		{
			size_t width = 0;
			size_t height = 0;
			VOMMAGetBufferInfo(buffer, BUF_INFO_WIDTH, &piType, &width, &piSize);
			VOMMAGetBufferInfo(buffer, BUF_INFO_HEIGHT, &piType, &height, &piSize);

			memcpy_s(data, width * height, pdata, width * height);
			VOMMABufferPush(hStream, buffer);
			return true;
		}
		VOMMABufferPush(hStream, buffer);
	}
	return false;
}

void CameraSetDepthCalPara(Variables vars)
{
	VOMMASetDepthCalPara(vars);
}

bool CameraGetDepthImg(uchar* data, size_t width, size_t height, uchar* depthData, const char* depthDir)
{
	size_t length = width * height * 3;
	_VOMMAImage vImage;
	vImage.imageData = new uchar[length];
	vImage.cols = width;
	vImage.rows = height;
	vImage.imageType = VOMMA_8UC1;
	memcpy_s(vImage.imageData, width * height, data, width * height);
	VOMMAGetDepthImg(vImage, depthDir, "depth", "center");
	memcpy_s(depthData, length, vImage.imageData, length);
	return true;
}

void RegisterImageReceived(CameraEventCallBack callBack)
{
	ImageReceived = callBack;
}
