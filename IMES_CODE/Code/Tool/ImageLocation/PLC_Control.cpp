// これは ?イ? DLL ファイ?です。
#include "PLC_Control.h"

// コ?スト?クタ
PLC_Control::PLC_Control():m_DstSocket(INVALID_SOCKET)
{
}

// デスト?クタ
PLC_Control::~PLC_Control()
{
	if(m_DstSocket != INVALID_SOCKET)
		closesocket(m_DstSocket);
	// Windows でのソケットの終了
	WSACleanup();
}

// ソケット接続
bool PLC_Control::SocketConnect(const char* IP,u_short PORT)
{
	if( WSAStartup(MAKEWORD(2,0), &wsdata) != 0) {
		return false;
	}

	// ソケットの生成
	m_DstSocket = socket(PF_INET, SOCK_STREAM, 0);
	if(m_DstSocket==INVALID_SOCKET) {
		return false;
	}

	// sockaddr_in 構造体のセット
	struct sockaddr_in dstAddr;
	memset(&dstAddr, 0, sizeof(dstAddr));
	dstAddr.sin_family = AF_INET;
	dstAddr.sin_port = htons(PORT);
	dstAddr.sin_addr.s_addr = inet_addr(IP);

	// 接続
	if(connect(m_DstSocket, (struct sockaddr *) &dstAddr, sizeof(dstAddr)) == SOCKET_ERROR) {
		return false;
	}

	// ソケットを非同期?ードにする
	u_long val=1;
	ioctlsocket(m_DstSocket, FIONBIO, &val);
	return true;
}

// ソケット?信
bool PLC_Control::SocketSend(char* pData,int DataSize)
{
	//パケットの?信
	if(send(m_DstSocket, pData, DataSize, 0) == SOCKET_ERROR) {
		return false;
	}
	return true;
}

// ソケット受信
RECVSTATUS PLC_Control::SocketRecv(char* pData,int DataSize,int *pRecvSize)
{
	int n = 1;
	char* pt=pData;

	//タイ?アウトを約5秒にするための?ープ
	while( n && SOCKET_ERROR != n && 0<(DataSize-(pData-pt))){
		for(int i=0;i<50;i++){
			n = recv(m_DstSocket, pData, DataSize-(pData-pt), 0 );		//受信
			if( 0 <= n ) {
				break;
			}
			Sleep(50);
		}
		pData += n;
	}
	if (n < 1) {
		// データが?ていない
		if (WSAGetLastError() == WSAEWOULDBLOCK) {
			return RECV_STILL;
		// 切断orエ?ー
		} else {
			return RECV_FAILED;
		}
	}
	*pRecvSize = n;	// 受信データ長取得
	return RECV_SUCCESSED;
}

// ソケット切断
bool PLC_Control::SocketDisconnect()
{
	closesocket(m_DstSocket);
	// Windows でのソケットの終了
	WSACleanup();

	return true;
}

// ?入した棚番?を要?
WORD PLC_Control::Request_EntryShelfNo()
{
	bool ret = 0;
	char SendBuffer[1024] =
	{
		0x50, 0x00, 0x00, 0xFF, 0xFF, 0x03, 0x00,
		0x0C,										// Packet Data Length Low
		0x00,										// Packet Data Length Hi
		0x10, 0x00,
		0x01, 0x04,									// Sub Command
		0x00, 0x00,
		0xE8, 0x03, 0x00,							// D0003E8(D1000):EntryShelfNo
		0xA8,										// D reg
		0x01,										// Length Low
		0x00										// Length Hi
	};

//	HANDLE hThread;
//	DWORD dwExCode, threadId;

	// ソケット接続
	ret = SocketConnect( "192.168.0.10", 8192 );
	if( ret == false ) {
	}

	// ソケット?信
	ret = SocketSend( &SendBuffer[0], 21 );
	if( ret == false ) {
	}

	// ソケット受信
	RECVSTATUS result;
	char ReceiveBuffer[1024];
	memset( &ReceiveBuffer[0], 0x00, 1024 );
	int ReceiveSize;
	result = SocketRecv( &ReceiveBuffer[0], 1024, &ReceiveSize );
	if( result != RECV_SUCCESSED ) {
	}

	// ソケット切断
	SocketDisconnect();

	byte lowdata = ReceiveBuffer[11];
	byte hidata = ReceiveBuffer[12];
	WORD wShelfNo = hidata << 8;
	wShelfNo = wShelfNo & 0xFF00;
	wShelfNo = wShelfNo | lowdata;

	return wShelfNo;
}

// 完了した?ークの排出要?
bool PLC_Control::Request_ExitWork(WORD ShelfNo, WORD Result)
{
	bool ret = 0;
	char SendBuffer[1024] =
	{
		0x50, 0x00, 0x00, 0xFF, 0xFF, 0x03, 0x00,
		0x12,										// Packet Data Length Low
		0x00,										// Packet Data Length Hi
		0x10, 0x00,
		0x01, 0x14,									// Sub Command
		0x00, 0x00,
		0x4C, 0x04, 0x00,							// D00044C(D1100):ShelfNo of ExitWork
													// D00044E(D1102):Result of ExitWork
		0xA8,										// D reg
		0x03,										// Length Low
		0x00,										// Length Hi
		0x00, 0x00,									// Change Data D1100
		0x00, 0x00,									// Change Data D1101
		0x00, 0x00									// Change Data D1102							
	};

//	HANDLE hThread;
//	DWORD dwExCode, threadId;

	// ソケット接続
	ret = SocketConnect( "192.168.0.10", 8192 );
	if( ret == false ) {
	}

	byte hidata = (byte)ShelfNo >> 8;
	byte lowdata = (byte)ShelfNo;
	SendBuffer[21] = lowdata;
	SendBuffer[22] = hidata;

	WORD work_result;
	if( Result == 1 ) {				// OK	
		work_result = 0x05FE;
	}
	else {							// NG
		work_result = 0x05FF;
	}
	hidata = (byte)work_result >> 8;
	lowdata = (byte)work_result;
	SendBuffer[25] = (char)lowdata;
	SendBuffer[26] = (char)hidata;

	// ソケット?信
	ret = SocketSend( &SendBuffer[0], 27 );
	if( ret == false ) {
	}

	// ソケット受信
	RECVSTATUS result;
	char ReceiveBuffer[1024];
	memset( &ReceiveBuffer[0], 0x00, 1024 );
	int ReceiveSize;
	result = SocketRecv( &ReceiveBuffer[0], 1024, &ReceiveSize );
	if( result != RECV_SUCCESSED ) {
	}

	// ソケット切断
	SocketDisconnect();

	return true;
}

// 全ての棚の状態を要?
bool PLC_Control::Request_All_ShelfStatus(WORD* ShelfStatus)
{
	bool ret = 0;
	char SendBuffer[1024] =
	{
		0x50, 0x00, 0x00, 0xFF, 0xFF, 0x03, 0x00,
		0x0C,										// Packet Data Length Low
		0x00,										// Packet Data Length Hi
		0x10, 0x00,
		0x01, 0x04,									// Sub Command
		0x00, 0x00,
		0xDC, 0x05, 0x00,							// D0005DC(D1500):EntryShelfNo
		0xA8,										// D reg
		0xFA,										// Length Low
		0x00										// Length Hi
	};

//	HANDLE hThread;
//	DWORD dwExCode, threadId;

	// ソケット接続
	ret = SocketConnect( "192.168.0.10", 8192 );
	if( ret == false ) {
	}

	// ソケット?信
	ret = SocketSend( &SendBuffer[0], 21 );
	if( ret == false ) {
	}

	// ソケット受信
	RECVSTATUS result;
	char ReceiveBuffer[1024];
	memset( &ReceiveBuffer[0], 0x00, 1024 );
	int ReceiveSize;
	result = SocketRecv( &ReceiveBuffer[0], 1024, &ReceiveSize );
	if( result != RECV_SUCCESSED ) {
	}

	// ソケット切断
	SocketDisconnect();

	for( int i=0 ; i<250 ; i++ ) {
		byte lowdata = ReceiveBuffer[11+i*2];
		byte hidata = ReceiveBuffer[12+i*2];
		*ShelfStatus = hidata << 8;
		*ShelfStatus = *ShelfStatus & 0xFF00;
		*ShelfStatus = *ShelfStatus | lowdata;
		ShelfStatus++;
	}

	return true;
}

PLC_Control* CreateInstance() {
return new PLC_Control ;
}
 
void ReleseInstance(PLC_Control* p) {
	delete p;
}
