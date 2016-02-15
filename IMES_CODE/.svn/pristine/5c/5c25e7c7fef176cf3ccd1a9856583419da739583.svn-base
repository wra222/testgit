
Function setCalDate(strDate)
	
	Dim strFormatDate
	Dim strYear
	Dim strMonth
	Dim strDay
	Dim firstSep
	Dim secondSep
	Dim current
	strFormatDate = FormatDateStr(strDate)
	If Not IsNull(strFormatDate) Then
		'If IsDate(strDate) Then
		'	strFormatDate = CStr(CDate(strDate))
		'End If
	Else
		current = Now
		strFormatDate = CStr(current)
	End If
	firstSep = Instr(strFormatDate, "-")
	secondSep = Instr(firstSep + 1, strFormatDate, "-")
	If (firstSep > 0) And (secondSep > firstSep) Then
		strYear = Left(strFormatDate, firstSep - 1)
		If Len(strYear) = 2 Then
			If CInt(strYear) < 30 Then
				strYear = "20" + strYear
			Else
				strYear = "19" + strYear
			End If
		End If
		strMonth = Mid(strFormatDate, firstSep + 1, secondSep - firstSep - 1)
		strDay = Mid(strFormatDate, secondSep + 1)
		document.frames.CalFrame.cal.year = strYear
		document.frames.CalFrame.cal.month = strMonth
		document.frames.CalFrame.cal.day = strDay
	End If
		
End Function

Function FormatDateStrOld(strDate)
	Dim strFormatDate
	Dim strYear
	Dim strMonth
	Dim strDay
	Dim firstSep
	Dim secondSep
	Dim nYear
	Dim nTempYear
	If IsDate(strDate) Then
		strFormatDate = CStr(CDate(strDate))
		firstSep = Instr(strFormatDate, "-")
		secondSep = Instr(firstSep + 1, strFormatDate, "-")
		If (firstSep > 0) And (secondSep > firstSep) Then
			strYear = Left(strFormatDate, firstSep - 1)
			If Len(strYear) = 2 Then
				nTempYear = CInt(strYear)
				If nTempYear < 30 Then
					strYear = "20" + strYear
				Else
					strYear = "19" + strYear
				End If
			End If
			strMonth = Mid(strFormatDate, firstSep + 1, secondSep - firstSep - 1)
			IF Len(strMonth) < 2 Then
				strMonth = "0" + strMonth
			End If
			strDay = Mid(strFormatDate, secondSep + 1)
			If Len(strDay) < 2 Then
				strDay = "0" + strDay
			End If
			If InStr(strDate, strYear) > 0 Then
				nYear = CInt(strYear)
				If nYear > 1900 And nYear < 2100 Then
					'FormatDateStr = strFormatDate
					FormatDateStr = strYear + "-" + strMonth + "-" + strDay
				Else
					FormatDateStr = Null
					'msgbox("The year " + strYear + " out of range.")
				End If
			Else 
				FormatDateStr = Null
				'msgbox(strDate + " isn't a right date format." + vbCrLf + "Mistake year string.")
			End If
		Else
			FormatDateStr = Null
			'msgbox(strDate + " isn't a right date format." )			
		End If
	Else
		FormatDateStr = Null
		'msgbox(strDate + " isn't a right date format." )			
	End If	
End Function

Function FormatDateStr(strDate)
	Dim strFormatDate
	Dim strYear
	Dim strMonth
	Dim strDay
	Dim firstSep
	Dim secondSep
	Dim nYear
	Dim nTempYear
	Dim dateSource
	If IsDate(strDate) Then
		dateSource = CDate(strDate)
		strYear = CStr(Year(dateSource))
		If Len(strYear) = 2 Then
			nTempYear = CInt(strYear)
			If nTempYear < 30 Then
				strYear = "20" + strYear
			Else
				strYear = "19" + strYear
			End If
		End If
		strMonth = CStr(Month(dateSource))
		IF Len(strMonth) < 2 Then
			strMonth = "0" + strMonth
		End If
		strDay = CStr(Day(dateSource))
		If Len(strDay) < 2 Then
			strDay = "0" + strDay
		End If
		If InStr(strDate, strYear) > 0 Then
			nYear = CInt(strYear)
			If nYear > 1900 And nYear < 2100 Then
				FormatDateStr = strYear & "-" + strMonth & "-" & strDay
			Else
				FormatDateStr = Null
			End If
		Else 
			FormatDateStr = Null
		End If
	Else
		FormatDateStr = Null
	End If
End Function
