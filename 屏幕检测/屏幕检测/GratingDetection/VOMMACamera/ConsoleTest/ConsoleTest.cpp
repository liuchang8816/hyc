
#include <iostream>
#include "VOMMACamera.h"
#include <opencv2/opencv.hpp>
using namespace cv;

void OnImageReceived(uchar* data, size_t width, size_t height) {
	std::cout << "-->" << "Image Received" << std::endl;
	std::cout << "-->" << "Width：" << width << std::endl;
	std::cout << "-->" << "Height：" << height << std::endl;
	Mat img = Mat(height, width, CV_8UC1, data);
	imwrite(".\\1.bmp", img);
}

int main()
{
	bool ret;

	char desc[64];
	ret = CameraSearch(desc);
	if (ret) {
		std::cout << "-->" << desc << std::endl;
	}
	else {
		std::cout << "-->" << "Camera Search Failed!" << std::endl;
	}

	ret = CameraOpen();
	if (ret) {
		std::cout << "-->" << "Camera Opened!" << std::endl;
	}
	else {
		std::cout << "-->" << "Camera Open Failed!" << std::endl;
	}

	int64_t exposureTime = 0;
	ret = CameraGetExposureTime(exposureTime);
	if (ret) {
		std::cout << "-->" << "ExposureTime:" << exposureTime << std::endl;
	}
	else {
		std::cout << "-->" << "Get ExposureTime Failed!" << std::endl;
	}
	getchar();

	ret = CameraSetExposureTime(exposureTime);
	if (ret) {
		std::cout << "-->" << "Set ExposureTime!" << std::endl;
	}
	else {
		std::cout << "-->" << "Set ExposureTime Failed!" << std::endl;
	}
	getchar();

	ret = CameraSetTriggerMode(1);
	if (ret) {
		std::cout << "-->" << "Set TriggerMode!" << std::endl;
	}
	else {
		std::cout << "-->" << "Set TriggerMode Failed!" << std::endl;
	}
	getchar();

	RegisterImageReceived(OnImageReceived);

	ret = CameraStartAcquisition();
	if (ret) {
		std::cout << "-->" << "Camera Start Acquisition!" << std::endl;
	}
	else {
		std::cout << "-->" << "Camera Start Acquisition Failed!" << std::endl;
	}
	getchar();

	CameraSoftTrigger();

	/*size_t width = 0;
	size_t height = 0;
	ret = CameraGetImageSize(width, height);
	if (ret) {
		std::cout << "-->" << "Width:" << width << " Height:" << height << std::endl;
	}
	else {
		std::cout << "-->" << "Get Image Size Failed!" << std::endl;
	}
	getchar();

	size_t length = width * height;
	uchar* data = new uchar[length];
	CameraGetImage(data);*/

	std::string input;
	std::cin >> input;
}

