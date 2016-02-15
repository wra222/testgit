// PLC_Control.h
#ifndef _PLC_CONTROL_H_
#define _PLC_CONTROL_H_

#include <winsock2.h>
#include <ws2tcpip.h>

#pragma comment(lib,"ws2_32.lib")

#include <windows.h>

#pragma once

#ifndef _RECVSTATUS_
#define _RECVSTATUS_

	// 受信状態
	enum RECVSTATUS
	{
		RECV_STILL,		// データが?ていない
		RECV_SUCCESSED,	// 成功
		RECV_FAILED		// 切断orエ?ー
	};
#endif

class PLC_Control
{
	private:
		SOCKET m_DstSocket;		// ?受信ソケット
		WSADATA wsdata;

		// 接続
		bool SocketConnect(const char* IP,u_short PORT);
	
		// 受信
		RECVSTATUS SocketRecv(char* pData,int DataSize,int *pRecvSize);

		// ?信
		bool SocketSend(char* pData,int DataSize);

		// 切断
		bool SocketDisconnect();

	public:
		// コ?スト?クタ
		PLC_Control();

		// デスト?クタ
		virtual ~PLC_Control();

		// ?入した棚番?を要?
		virtual WORD Request_EntryShelfNo();

		// 完了した?ークの排出要?
		virtual bool Request_ExitWork(WORD ShelfNo, WORD Result);

		// 全ての棚の状態を要?
		virtual bool Request_All_ShelfStatus(WORD* ShelfStatus);

	public:
		friend __declspec(dllexport) PLC_Control* CreateInstance();
		friend __declspec(dllexport) void ReleseInstance(PLC_Control* p);
};

PLC_Control* CreateInstance();
void ReleseInstance(PLC_Control* p);

#endif
