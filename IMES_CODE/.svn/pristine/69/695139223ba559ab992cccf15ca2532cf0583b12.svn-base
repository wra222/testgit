#pragma once
#include <stdio.h>
#include <string.h>
#include <windows.h>

#pragma comment(lib,"ws2_32.lib") //Winsock Library

int loadSocket()
{
	 WSADATA wsa;
     if (WSAStartup(MAKEWORD(2,2),&wsa) != 0)
    {		
        return -1;
	}	
	return 0;
}
void unloadSocket()
{	
	WSACleanup();
}

int sendData(char* ip, int port, char* sendData, int sendSize,char* recvData)
{
    //WSADATA wsa;
    SOCKET _socket;
    struct sockaddr_in server;
	int orgRecvSize;
     
    //Create a socket
    if((_socket = socket(AF_INET , SOCK_STREAM , 0 )) == INVALID_SOCKET)
    {
		return -2;
    }
 
    server.sin_addr.s_addr = inet_addr(ip);
    server.sin_family = AF_INET;
	server.sin_port = htons(port);
 
    //Connect to remote server
    if (connect(_socket , (struct sockaddr *)&server , sizeof(server)) < 0)
    {
       return -3;
    }     
    int timeout=10;
	setsockopt(_socket , SOL_SOCKET, SO_RCVTIMEO, (const char*)&timeout,  sizeof(INT));

    //Send some data
	if( send(_socket , sendData , sendSize , 0) < 0)
	{      
		closesocket(_socket );
		//WSACleanup();
		return -4;
	}

	
 
    //Receive a reply from the server 
	char dataIn[1024];
	memset( dataIn, 0x00, 1024 );
	 if((orgRecvSize = recv(_socket, dataIn , 1024 , 0)) == SOCKET_ERROR)
    {		
		closesocket(_socket);
        return -5;
    } 

	int recvSize=orgRecvSize+1;
    memcpy(recvData, dataIn, recvSize);
	recvData[recvSize] ='\0';  
  
	closesocket(_socket );
    //WSACleanup();
	return orgRecvSize;
}

int main(int argc , char *argv[])
{	
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
	
	 int ret =loadSocket();	
	 if (ret<0)
	{
			
			return ret;
	}
	
	  
	char ReceiveBuffer[1024];
	ret = sendData("192.168.0.10", 8192,SendBuffer, 21,ReceiveBuffer);
	 if (ret<0)
	{			
		return ret;
	}
    
	byte lowdata = ReceiveBuffer[11];
	byte hidata = ReceiveBuffer[12];
	WORD wShelfNo = hidata << 8;
	wShelfNo = wShelfNo & 0xFF00;
	wShelfNo = wShelfNo | lowdata;

	unloadSocket(); //unload Socket
	
	return wShelfNo;
}
 
 