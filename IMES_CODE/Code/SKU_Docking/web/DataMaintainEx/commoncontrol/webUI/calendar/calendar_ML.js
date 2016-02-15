
//------------------------------------------------------------------------
// Attach to element events
//------------------------------------------------------------------------

element.attachEvent("onselectstart", fnOnSelectStart)
element.attachEvent("onclick", fnOnClick)
element.attachEvent("onpropertychange", fnOnPropertyChange)
element.attachEvent("onreadystatechange", fnOnReadyStateChange)

//------------------------------------------------------------------------
// Create the arrays of days & months for different languages
//------------------------------------------------------------------------
//Risun changed to Chinese. 2000.8.5
/*var gaMonthNames = new Array(
  new Array('Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
            'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'),
  new Array('January', 'February', 'March', 'April', 'May', 'June', 'July',
            'August', 'September', 'October', 'November', 'December')
  );
*/

//==========================================
//Risun changed for multi language. 2001.7.19
//------------------------------------------
var gaMonthNames_English = new Array(
  new Array('Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
            'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'),
  new Array('January', 'February', 'March', 'April', 'May', 'June', 'July',
            'August', 'September', 'October', 'November', 'December')
  );

var gaMonthNames_SimChinese = new Array(
  new Array('\u4e00\u6708', '\u4e8c\u6708', '\u4e09\u6708', '\u56db\u6708', '\u4e94\u6708', '\u516d\u6708',
            '\u4e03\u6708', '\u516b\u6708', '\u4e5d\u6708', '\u5341\u6708', '\u5341\u4e00\u6708', '\u5341\u4e8c\u6708'),
  new Array('\u4e00\u6708', '\u4e8c\u6708', '\u4e09\u6708', '\u56db\u6708', '\u4e94\u6708', '\u516d\u6708', '\u4e03\u6708',
            '\u516b\u6708', '\u4e5d\u6708', '\u5341\u6708', '\u5341\u4e00\u6708', '\u5341\u4e8c\u6708')
  );

var gaMonthNames_TraChinese = new Array(
  new Array('\ue5f2\u308b', '\ue5f9\u308b', '\ue606\u308b', '\ue699\u308b', '\u304d\u308b', '\u305b\u308b',
            '\ue5f5\u308b', '\ue5fd\u308b', '\ue5f7\u308b', '\ue603\u308b', '\ue603\ue5f2\u308b', '\ue603\ue5f9\u308b'),
  new Array('\ue5f2\u308b', '\ue5f9\u308b', '\ue606\u308b', '\ue699\u308b', '\u304d\u308b', '\u305b\u308b', '\ue5f5\u308b',
            '\ue5fd\u308b', '\ue5f7\u308b', '\ue603\u308b', '\ue603\ue5f2\u308b', '\ue603\ue5f9\u308b')
  );

var gaMonthNames = new Array(
  new Array('\u4e00\u6708', '\u4e8c\u6708', '\u4e09\u6708', '\u56db\u6708', '\u4e94\u6708', '\u516d\u6708',
            '\u4e03\u6708', '\u516b\u6708', '\u4e5d\u6708', '\u5341\u6708', '\u5341\u4e00\u6708', '\u5341\u4e8c\u6708'),
  new Array('\u4e00\u6708', '\u4e8c\u6708', '\u4e09\u6708', '\u56db\u6708', '\u4e94\u6708', '\u516d\u6708', '\u4e03\u6708',
            '\u516b\u6708', '\u4e5d\u6708', '\u5341\u6708', '\u5341\u4e00\u6708', '\u5341\u4e8c\u6708')
  );
//------------------------------------------

/*
var gaDayNames = new Array(
  new Array('S', 'M', 'T', 'W', 'T', 'F', 'S'),
  new Array('Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'),
  new Array('Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday')
  );
*/
//==========================================
//Risun changed for multi language. 2001.7.19
//------------------------------------------
var gaDayNames_English = new Array(
  new Array('S', 'M', 'T', 'W', 'T', 'F', 'S'),
  new Array('Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'),
  new Array('Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday')
  );
var gaDayNames_SimChinese = new Array(
  new Array('\u65e5','\u4e00', '\u4e8c', '\u4e09', '\u56db', '\u4e94', '\u516d'),
  new Array('\u65e5','\u4e00', '\u4e8c', '\u4e09', '\u56db', '\u4e94', '\u516d'),
  new Array('\u661f\u671f\u65e5','\u661f\u671f\u4e00', '\u661f\u671f\u4e8c', '\u661f\u671f\u4e09', '\u661f\u671f\u56db', '\u661f\u671f\u4e94', '\u661f\u671f\u516d')
  );
var gaDayNames_TraChinese = new Array(
  new Array('\u3089','\ue5f2', '\ue5f9', '\ue606', '\ue699', '\u304d', '\u305b'),
  new Array('\u3089','\ue5f2', '\ue5f9', '\ue606', '\ue699', '\u304d', '\u305b'),
  new Array('\u740d\u6233\u3089','\u740d\u6233\ue5f2', '\u740d\u6233\ue5f9', '\u740d\u6233\ue606', '\u740d\u6233\ue699', '\u740d\u6233\u304d', '\u740d\u6233\u305b')
  );
//------------------------------------------


var gaDayNames = new Array(
  new Array('\u65e5','\u4e00', '\u4e8c', '\u4e09', '\u56db', '\u4e94', '\u516d'),
  new Array('\u65e5','\u4e00', '\u4e8c', '\u4e09', '\u56db', '\u4e94', '\u516d'),
  new Array('\u661f\u671f\u65e5','\u661f\u671f\u4e00', '\u661f\u671f\u4e8c', '\u661f\u671f\u4e09', '\u661f\u671f\u56db', '\u661f\u671f\u4e94', '\u661f\u671f\u516d')
  );
//------------------------------------------

var gaMonthDays = new Array(
   /* Jan */ 31,     /* Feb */ 29, /* Mar */ 31,     /* Apr */ 30,
   /* May */ 31,     /* Jun */ 30, /* Jul */ 31,     /* Aug */ 31,
   /* Sep */ 30,     /* Oct */ 31, /* Nov */ 30,     /* Dec */ 31 )

var StyleInfo            = null            // Style sheet with rules for this calendar
var goStyle              = new Object()    // A hash of style sheet rules that apply to this calendar
var gaDayCell            = new Array()     // an array of the table cells for days
var goDayTitleRow        = null            // The table row containing days of the week
var goYearSelect         = null            // The year select control
var goMonthSelect        = null            // The month select control
var goCurrentDayCell     = null            // The cell for the currently selected day
var giStartDayIndex      = 0               // The index in gaDayCell for the first day of the month
var gbLoading            = true            // Flag for if the behavior is loading

var giDay                                  // day of the month (1 to 31)
var giMonth                                // month of the year (1 to 12)
var giYear                                 // year (1900 to 2099)

var giMonthLength        = 1               // month length (0,1)
var giDayLength          = 1               // day length (0 to 2)
var giFirstDay           = 0               // first day of the week (0 to 6)
var gsGridCellEffect     = 'raised'        // Grid cell effect
var gsGridLinesColor     = 'black'         // Grid line color
var gbShowDateSelectors  = true            // Show date selectors (0,1)
var gbShowDays           = true            // Show the days of the week titles (0,1)
var gbShowTitle          = true            // Show the title (0,1)
var gbShowHorizontalGrid = true            // Show the horizontal grid (0,1)
var gbShowVerticalGrid   = true            // Show the vertical grid (0,1)
var gbValueIsNull        = false           // There is no value selected (0,1)
var gbReadOnly           = false           // The user can not interact with the control

var giMinYear            = 1900            // Minimum year (1 is the lowest possible value)
var giMaxYear            = 2099            // Maximum year
//==========================================
//Risun add for multi language. 2001.7.19
//------------------------------------------
var giLanguage           = -1              // 0 = English; 1 = simplified; 2 = traditional;
                                           // -1 = detect by self
giLanguage = fnCheckCharset()

//==========================================
// Load the property values defined on the element to replace defaults
fnGetPropertyDefaults()

// Create the style sheets needed for the calendar display
fnCreateStyleSheets()

// Insert the HTML elements needed for the calendar display
fnCreateCalendarHTML()

// Update the title with the month and year
fnUpdateTitle()

// Fill in the days of the week
fnUpdateDayTitles()

// Build the month select control
fnBuildMonthSelect()

// Build the year select control
fnBuildYearSelect()

// Fill in the cells with the days of the month and set style values
fnFillInCells()

// **********************************************************************
//                       PROPERTY GET/SET FUNCTIONS
// **********************************************************************


//------------------------------------------------------------------------
//
//  Function:  fnGetDay / fnPutDay
//
//  Synopsis:  The day property is used to set the day of the month.  The
//             valid range is from 1 to the maximum day of the selected
//             month & year.  If a number is given outside that range, it
//             is set to the closest valid value.  Invalid input will cause
//             an exception.
//
//  Arguments: The put method requires an integer value for the day
//
//  Returns:   The get method will return the selected day of the month
//             If the valueIsNull property is set, null is returned
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetDay()
{
  return (gbValueIsNull) ? null : giDay
}

function fnPutDay(iDay)
{
  if (gbLoading) return  // return if the behavior is loading

  iDay = parseInt(iDay, 10)
  if (isNaN(iDay)) throw 450

  fnSetDate(iDay, giMonth, giYear)
}

//------------------------------------------------------------------------
//
//  Function:  fnGetMonth / fnPutMonth
//
//  Synopsis:  The month property is used to set the month of the year.
//             The valid range is from 1 to 12.  If a value is given
//             outside that range, it is set to the closest valid value.
//             Invalid input will cause an exception.
//
//  Arguments: The put method requires an integer value for the month
//
//  Returns:   The get method will return the selected month value
//             If the valueIsNull property is set, null is returned
//
//  Notes:     Setting the year can cause the selected "day" value to be
//             reduced to the highest day in the selected month if needed.
//
//------------------------------------------------------------------------

function fnGetMonth()
{
  return (gbValueIsNull) ? null : giMonth
}

function fnPutMonth(iMonth)
{
  if (gbLoading) return  // return if the behavior is loading

  iMonth = parseInt(iMonth, 10)
  if (isNaN(iMonth)) throw 450

  fnSetDate(giDay, iMonth, giYear)
}

//------------------------------------------------------------------------
//
//  Function:  fnGetYear / fnPutYear
//
//  Synopsis:  The year property is used to set the current year.
//             The valid range is from minYear to maxYear.  If a value is given
//             outside that range, it is set to the closest valid value.
//             Invalid input will cause an exception.
//
//  Arguments: The put method requires an integer value for the year
//
//  Returns:   The get method will return the selected year value
//             If the valueIsNull property is set, null is returned.
//
//  Notes:     Setting the year can cause the selected "day" value to be
//             reduced to the highest day in the selected month if needed.
//
//------------------------------------------------------------------------

function fnGetYear()
{
  return (gbValueIsNull) ? null : giYear
}

function fnPutYear(iYear)
{
  if (gbLoading) return  // return if the behavior is loading

  iYear = parseInt(iYear, 10)
  if (isNaN(iYear)) throw 450

  fnSetDate(giDay, giMonth, iYear)
}

//------------------------------------------------------------------------
//
//  Function:  fnGetMonthLength / fnPutMonthLength
//
//  Synopsis:  The monthLength property is used to adjust the length of
//             the month name used in the title and month selection control.
//
//  Arguments: The put method requires a value of 'short' or 'long'
//
//  Returns:   The get method will return a value of 'short' or 'long'
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetMonthLength()
{
  if (giMonthLength == 0) return "short"
  if (giMonthLength == 1) return "long"
}

function fnPutMonthLength(sLength)
{
  if (gbLoading) return  // return if the behavior is loading

  switch (sLength.toLowerCase())
  {
    case "short" :
      if (giMonthLength == 0) return
      giMonthLength = 0
      break;
    case "long" :
      if (giMonthLength == 1) return
      giMonthLength = 1
      break;
    default :
      throw 450
      return
  }

  fnUpdateTitle()
  fnBuildMonthSelect()
}

//------------------------------------------------------------------------
//
//  Function:  fnGetDayLength / fnPutDayLength
//
//  Synopsis:  The dayLength property is used to adjust the length of
//             the day names in the calendar grid.
//
//  Arguments: The put method requires a value of 'short', 'medium',
//             or 'long'
//
//  Returns:   The get method will return a value of 'short', 'medium',
//             or 'long'
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetDayLength()
{
  if (giDayLength == 0) return "short"
  if (giDayLength == 1) return "medium"
  if (giDayLength == 2) return "long"
}

function fnPutDayLength(sLength)
{
  if (gbLoading) return  // return if the behavior is loading

  switch (sLength.toLowerCase())
  {
    case "short" :
      if (giDayLength == 0) return
      giDayLength = 0
      break;
    case "medium" :
      if (giDayLength == 1) return
      giDayLength = 1
      break;
    case "long" :
      if (giDayLength == 2) return
      giDayLength = 2
      break;
    default :
      throw 450
      return
  }

  fnUpdateDayTitles()

  // Used to force a table resize if needed
  goStyle['DaySelected'].borderStyle = 'solid'
}

//------------------------------------------------------------------------
//
//  Function:  fnGetFirstDay / fnPutFirstDay
//
//  Synopsis:  The firstDay property is used to adjust the first day of
//             the week on the calendar grid.  Valid values are 1 to 7
//             where 1 is Sunday and 7 is Saturday.  Setting to an invalid
//             value will cause an exception.
//
//  Arguments: The put method requires a value from 1 to 7
//
//  Returns:   The get method will return a value from 1 to 7
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetFirstDay()
{
  return giFirstDay
}

function fnPutFirstDay(iFirstDay)
{
  if (gbLoading) return  // return if the behavior is loading
  if ((iFirstDay < 0) || (iFirstDay > 6)) throw 450
  if (giFirstDay == iFirstDay) return
  giFirstDay = iFirstDay

  fnUpdateDayTitles()
  fnFillInCells()
}

//------------------------------------------------------------------------
//
//  Function:  fnGetGridCellEffect / fnPutGridCellEffect
//
//  Synopsis:  The gridCellEffect property is used to modify the 3D effect
//             in the calendar grid (excluding day titles).  It can take
//             values of 'raised', 'flat', or 'sunken'.  Other values will
//             cause an exception.
//
//  Arguments: The put method requires a value of 'raised', 'flat', or
//             'sunken'.
//
//  Returns:   The get method will return a value of 'raised', 'flat', or
//             'sunken'.
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetGridCellEffect()
{
  return gsGridCellEffect
}

function fnPutGridCellEffect(sEffect)
{
  if (gbLoading) return  // return if the behavior is loading

  switch (sEffect.toLowerCase())
  {
    case "raised" :
      if (gsGridCellEffect == 'raised') return
      gsGridCellEffect = 'raised'
      fnUpdateGridColors()
      break
    case "flat" :
      if (gsGridCellEffect == 'flat') return
      gsGridCellEffect = 'flat'
      fnUpdateGridColors()
      break
    case "sunken" :
      if (gsGridCellEffect == 'sunken') return
      gsGridCellEffect = 'sunken'
      fnUpdateGridColors()
      break
    default :
      throw 450
  }
}

//------------------------------------------------------------------------
//
//  Function:  fnGetGridLinesColor / fnPutGridLinesColor
//
//  Synopsis:  The gridLinesColor property is used to change the color of
//             the calendar grid when the gridCellEffect property is set
//             to 'flat'.  It can be any valid HTML color value.
//
//  Arguments: The put method requires a HTML color value
//
//  Returns:   The get method will return a HTML color value
//
//  Notes:     No error checking is performed.  Invalid values may result
//             in unexpected rendering.
//
//------------------------------------------------------------------------

function fnGetGridLinesColor()
{
  return gsGridLinesColor
}

function fnPutGridLinesColor(sGridLinesColor)
{
  if (gbLoading) return  // return if the behavior is loading

  gsGridLinesColor = sGridLinesColor
  fnUpdateGridColors()
}

//------------------------------------------------------------------------
//
//  Function:  fnGetShowVerticalGrid / fnPutShowVerticalGrid
//
//  Synopsis:  The showVerticalGrid property is used to toggle the
//             visibility of vertical lines in the calendar grid.
//
//  Arguments: The put method requires true or false value for visibility
//
//  Returns:   The get method will return a true or false value
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetShowVerticalGrid()
{
  return gbShowVerticalGrid
}

function fnPutShowVerticalGrid(bShowVerticalGrid)
{
  if (gbLoading) return  // return if the behavior is loading

  if ((bShowVerticalGrid) != gbShowVerticalGrid)
  {
    gbShowVerticalGrid = (bShowVerticalGrid) ? true : false
    fnFireOnPropertyChange("propertyName", "showVerticalGrid")
    fnUpdateGridColors()
  }
}


//------------------------------------------------------------------------
//
//  Function:  fnGetShowHorizontalGrid / fnPutShowHorizontalGrid
//
//  Synopsis:  The showHorizontalGrid property is used to toggle the
//             visibility of horizontal lines in the calendar grid.
//
//  Arguments: The put method requires true or false value for visibility
//
//  Returns:   The get method will return a true or false value
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetShowHorizontalGrid()
{
  return gbShowHorizontalGrid
}

function fnPutShowHorizontalGrid(bShowHorizontalGrid)
{
  if (gbLoading) return  // return if the behavior is loading

  if ((bShowHorizontalGrid) != gbShowHorizontalGrid)
  {
    gbShowHorizontalGrid = (bShowHorizontalGrid) ? true : false
    fnFireOnPropertyChange("propertyName", "showHorizontalGrid")
    fnUpdateGridColors()
  }
}

//------------------------------------------------------------------------
//
//  Function:  fnGetShowDateSelectors / fnPutShowDateSelectors
//
//  Synopsis:  The showDateSelectors property toggles the visibility of
//             the month and year select controls.
//
//  Arguments: The put method requires true or false value for visibility
//
//  Returns:   The get method will return a true or false value
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetShowDateSelectors()
{
  return gbShowDateSelectors
}

function fnPutShowDateSelectors(bShowDateSelectors)
{
  if (gbLoading) return  // return if the behavior is loading

  gbShowDateSelectors = (bShowDateSelectors) ? true : false
  element.children[0].rows[0].cells[1].style.display = (gbShowDateSelectors) ? '' : 'none'

  element.children[0].rows[0].style.display = (gbShowDateSelectors || gbShowTitle) ? '' : 'none'
}

//------------------------------------------------------------------------
//
//  Function:  fnGetShowDays / fnPutShowDays
//
//  Synopsis:  The showDays property toggles the visibility of
//             the day of the week titles row in the calendar grid.
//
//  Arguments: The put method requires true or false value for visibility
//
//  Returns:   The get method will return a true or false value
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetShowDays()
{
  return gbShowDays
}

function fnPutShowDays(bShowDays)
{
  if (gbLoading) return  // return if the behavior is loading

  gbShowDays = (bShowDays) ? true : false
  goDayTitleRow.style.display = (gbShowDays) ? '' : 'none'
}

//------------------------------------------------------------------------
//
//  Function:  fnGetShowTitle / fnPutShowTitle
//
//  Synopsis:  the showTitle property toggles the visibility of the month
//             and year title at the top of the calendar.
//
//  Arguments: The put method requires true or false value for visibility
//
//  Returns:   The get method will return a true or false value
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetShowTitle()
{
  return gbShowTitle
}

function fnPutShowTitle(bShowTitle)
{
  if (gbLoading) return  // return if the behavior is loading

  gbShowTitle = (bShowTitle) ? true : false
  element.children[0].rows[0].style.display = (gbShowDateSelectors || gbShowTitle) ? '' : 'none'
  fnUpdateTitle()
}

//------------------------------------------------------------------------
//
//  Function:  fnGetValue / fnPutValue
//
//  Synopsis:  The value property returns the day, month, and year in the
//             format:     {day}/{month}/{year}
//             example:    25/02/1998
//             An invalid value will cause an exception.
//
//  Arguments: The put method requires a string in the format described
//             above
//
//  Returns:   The get method will return the current date value
//
//  Notes:     The day and month are returned in the two digit format.
//             The year is returned in a four digit format.
//
//------------------------------------------------------------------------

function fnGetValue()
{
  var sValue

  if (gbValueIsNull) return null
  sValue = ((giDay < 10) ? '0' + giDay : giDay) + '/' +
           ((giMonth < 10) ? '0' + giMonth : giMonth) + '/'
  if (giYear < 10) return sValue + '000' + giYear
  if (giYear < 100) return sValue + '00' + giYear
  if (giYear < 1000) return sValue + '0' + giYear
  return sValue + giYear
}

function fnPutValue(sValue)
{
  if (gbLoading) return  // return if the behavior is loading

  var aValue = sValue.split('/')

  // ensure valid valuse for month, day, and year
  aValue[0]++ ; aValue[0]-- ; aValue[1]++ ; aValue[1]-- ; aValue[2]++ ; aValue[2]-- ;
  if ( isNaN(aValue[0]) || isNaN(aValue[1]) || isNaN(aValue[2]) ) throw 450

  fnSetDate(aValue[0], aValue[1], aValue[2])
}


//------------------------------------------------------------------------
//
//  Function:  fnGetValueIsNull / fnPutValueIsNull
//
//  Synopsis:  The valueIsNull property set the calendar so that no day
//             is selected.  Changing any date property, setting one of
//             the date select controls, or clicking on a day will result
//             in the valueIsNull property getting set to false.
//
//  Arguments: The put method requires a string in the format described
//             above
//
//  Returns:   The get method will return the current date value
//
//  Notes:     The day and month are returned in the two digit format.
//             The year is returned in a four digit format.
//
//------------------------------------------------------------------------

function fnGetValueIsNull()
{
  return gbValueIsNull
}

function fnPutValueIsNull(bValueIsNull)
{
  if (gbLoading) return  // return if the behavior is loading

  if ((bValueIsNull) != gbValueIsNull)
  {
    gbValueIsNull = (bValueIsNull) ? true : false
    fnFireOnPropertyChange("propertyName", "readOnly")
  }

  goCurrentDayCell.className = (bValueIsNull) ?
    'Day_' + uniqueID : 'DaySelected_' + uniqueID
}

//------------------------------------------------------------------------
//
//  Function:  fnGetReadOnly / fnPutReadOnly
//
//  Synopsis:  The readOnly property can be set to true or false to
//             disable user date selection by clicking on days or through
//             the select controls
//
//  Arguments: The put method requires a true/false value
//
//  Returns:   The get method will return true or false
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetReadOnly()
{
  return (gbReadOnly) ? true : false
}

function fnPutReadOnly(bReadOnly)
{
  if (gbLoading) return  // return if the behavior is loading

  if ((bReadOnly) != gbReadOnly)
  {
    gbReadOnly = (bReadOnly) ? true : false
    fnFireOnPropertyChange("propertyName", "readOnly")
  }

  element.children[0].rows[0].cells[1].children[0].children[0].disabled = gbReadOnly
  element.children[0].rows[0].cells[1].children[0].children[1].disabled = gbReadOnly
}

// **********************************************************************
//                       CALENDAR INITIALIZATION FUNCTIONS
// **********************************************************************

//------------------------------------------------------------------------
//
//  Function:  fnCreateCalendarHTML
//
//  Synopsis:  This function adds the HTML code to the main document that
//             is required to display the calendar.  It contains nested
//             tables and all style information is inherited from the
//             style sheet properties.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnCreateCalendarHTML()
{
  var row, cell

  element.innerHTML =
  '<table border=0 class=WholeCalendar_' + uniqueID + '> ' +
  '  <tr>                                          ' +
  '      <td class=Title_' + uniqueID + '></td>    ' +
  '      <td class=DateControls_' + uniqueID + '>  ' +
  '        <nobr> <select></select>                ' +
  '               <select></select> </nobr> </td>  ' +
  '  </tr>                                         ' +
  '  <tr> <td colspan=3>                           ' +
  '    <table class=CalTable_' + uniqueID + ' cellspacing=0 border=0> ' +
  '      <tr><td class=DayTitle_' + uniqueID + ' style="color:red"></td>' +
  '          <td class=DayTitle_' + uniqueID + '></td>' +
  '          <td class=DayTitle_' + uniqueID + '></td>' +
  '          <td class=DayTitle_' + uniqueID + '></td>' +
  '          <td class=DayTitle_' + uniqueID + '></td>' +
  '          <td class=DayTitle_' + uniqueID + '></td>' +
  '          <td class=DayTitle_' + uniqueID + ' style="color:red"></td></tr>' +
  '      <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>' +
  '      <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>' +
  '      <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>' +
  '      <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>' +
  '      <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>' +
  '      <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>' +
  '    </table> ' +
  '  </tr>      ' +
  '</table>     ';

  goDayTitleRow = element.children[0].rows[1].cells[0].children[0].rows[0]
  goMonthSelect = element.children[0].rows[0].cells[1].children[0].children[0]
  goYearSelect = element.children[0].rows[0].cells[1].children[0].children[1]

  for (row=1; row < 7; row++)
    for (cell=0; cell < 7; cell++)
      gaDayCell[((row-1)*7) + cell] = element.children[0].rows[1].cells[0].children[0].rows[row].cells[cell]

}

//------------------------------------------------------------------------
//
//  Function:  fnCreateStyleSheets
//
//  Synopsis:  The calendar uses a style sheet to control the rendering
//             of the different calendar elements.  This function creates
//             the style sheet in the main document using a unique name.
//
//  Arguments: none
//
//  Returns:
//
//  Notes:
//
//------------------------------------------------------------------------

function fnCreateStyleSheets()
{
  var StyleInfo

  if (! element.document.body.BehaviorStyleSheet)
  {
    element.document.body.BehaviorStyleSheet = element.document.createStyleSheet()
  }
  StyleInfo = element.document.body.BehaviorStyleSheet

  StyleInfo.addRule(   '.WholeCalendar_' + uniqueID,
      'background-color : cornsilk         ;'+
      'border           : 0px solid black  ;'+
      'cursor           : default          ;'+
      'width            : 100%             ;'+
      'height           : 100%             ;'
    )
  goStyle['WholeCalendar'] = StyleInfo.rules[StyleInfo.rules.length - 1].style

  StyleInfo.addRule(   '.Title_' + uniqueID,
      'color            : #00009F  ;'+	// cal--title-color
      'font-family      : Arial    ;'+	// cal--title-font-family
      'font-size        : 10pt     ;'+	// cal--title-font-size
      'font-weight      : bold     ;'+	// cal--title-font-weight
      'text-align       : center   ;'+	// cal--title-text-align
      'height           : 1        ;'+
      'width            : 100%     ;'+
      'background-color : cornsilk;'
    )
  goStyle['Title'] = StyleInfo.rules[StyleInfo.rules.length - 1].style

  fnLoadCSSDefault('cal--title-background-color', 'calTitleBackgroundColor', goStyle['Title'], 'backgroundColor')
  fnLoadCSSDefault('cal--title-color',            'calTitleColor',           goStyle['Title'], 'color')
  fnLoadCSSDefault('cal--title-font-family',      'calTitleFontFamily',      goStyle['Title'], 'fontFamily')
  fnLoadCSSDefault('cal--title-font-size',        'calTitleFontSize',        goStyle['Title'], 'fontSize')
  fnLoadCSSDefault('cal--title-font-weight',      'calTitleFontWeight',      goStyle['Title'], 'fontWeight')
  fnLoadCSSDefault('cal--title-text-align',       'calTitleTextAlign',       goStyle['Title'], 'textAlign')

  StyleInfo.addRule(   '.DateControls_' + uniqueID,
      'text-align : right ;'
    )
  goStyle['DateControls'] = StyleInfo.rules[StyleInfo.rules.length - 1].style

  StyleInfo.addRule(   '.CalTable_' + uniqueID,
      'border : 1 solid black ;'+
      'width  : 100%          ;'+
      'height : 100%          ;'
    )
  goStyle['CalTable'] = StyleInfo.rules[StyleInfo.rules.length - 1].style

  StyleInfo.addRule(   '.DayTitle_' + uniqueID,
      'background-color    : cornsilk ;'+	// dayTitle-background-color
      'color               : black     ;'+	// dayTitle-color
      'font-family         : Arial     ;'+	// dayTitle-font-family
      'font-size           : 8pt       ;'+	// dayTitle-font-size
      'font-weight         : bold      ;'+	// dayTitle-font-weight
      'text-align          : center    ;'+	// dayTitle-text-align
      'border-width        : 1px       ;'+
      'border-style        : solid     ;'+
      'border-left-color   : white     ;'+
      'border-top-color    : white     ;'+
      'border-right-color  : black     ;'+
      'border-bottom-color : black     ;'+
      'width               : 14%       ;'+
      'height              : 1         ;'
    )
  goStyle['DayTitle'] = StyleInfo.rules[StyleInfo.rules.length - 1].style

  fnLoadCSSDefault('cal--dayTitle-background-color', 'calDayTitleBackgroundColor', goStyle['DayTitle'], 'backgroundColor')
  fnLoadCSSDefault('cal--dayTitle-color',            'calDayTitleColor',           goStyle['DayTitle'], 'color')
  fnLoadCSSDefault('cal--dayTitle-font-family',      'calDayTitleFontFamily',      goStyle['DayTitle'], 'fontFamily')
  fnLoadCSSDefault('cal--dayTitle-font-size',        'calDayTitleFontSize',        goStyle['DayTitle'], 'fontSize')
  fnLoadCSSDefault('cal--dayTitle-font-weight',      'calDayTitleFontWeight',      goStyle['DayTitle'], 'fontWeight')
  fnLoadCSSDefault('cal--dayTitle-text-align',       'calDayTitleTextAlign',       goStyle['DayTitle'], 'textAlign')

  StyleInfo.addRule(   '.OffDay_' + uniqueID,
      'background-color    : cornsilk ;'+	// cal--offMonth-background-color
      'color               : #7F7F7F   ;'+	// cal--offMonth-color
      'font-family         : Arial     ;'+	// cal--offMonth-font-family
      'font-size           : 8pt       ;'+	// cal--offMonth-font-size
      'font-weight         : normal    ;'+	// cal--offMonth-font-weight
      'text-align          : right     ;'+	// cal--offMonth-text-align
      'vertical-align      : text-top  ;'+	// cal--offMonth-vertical-align
      'border-width        : 1px       ;'+
      'border-style        : solid     ;'+
      'border-left-color   : white     ;'+
      'border-top-color    : white     ;'+
      'border-right-color  : black     ;'+
      'border-bottom-color : black     ;'+
      'width               : 14%       ;'+
      'cursor              : hand      ;'
    )
  goStyle['OffDay'] = StyleInfo.rules[StyleInfo.rules.length - 1].style

  fnLoadCSSDefault('cal--offMonth-background-color', 'calOffMonthBackgroundColor', goStyle['OffDay'], 'backgroundColor')
  fnLoadCSSDefault('cal--offMonth-color',            'calOffMonthColor',           goStyle['OffDay'], 'color')
  fnLoadCSSDefault('cal--offMonth-font-family',      'calOffMonthFontFamily',      goStyle['OffDay'], 'fontFamily')
  fnLoadCSSDefault('cal--offMonth-font-size',        'calOffMonthFontSize',        goStyle['OffDay'], 'fontSize')
  fnLoadCSSDefault('cal--offMonth-font-weight',      'calOffMonthFontWeight',      goStyle['OffDay'], 'fontWeight')
  fnLoadCSSDefault('cal--offMonth-text-align',       'calOffMonthTextAlign',       goStyle['OffDay'], 'textAlign')
  fnLoadCSSDefault('cal--offMonth-vertical-align',   'calOffMonthVerticalAlign',   goStyle['OffDay'], 'verticalAlign')

  StyleInfo.addRule(   '.Day_' + uniqueID,
      'background-color    : cornsilk ;'+	// cal--currentMonth-background-color
      'color               : #00009F   ;'+	// cal--currentMonth-color
      'font-family         : Arial     ;'+	// cal--currentMonth-font-family
      'font-size           : 8pt       ;'+	// cal--currentMonth-font-size
      'font-weight         : normal    ;'+	// cal--currentMonth-font-weight
      'text-align          : right     ;'+ 	// cal--currentMonth-text-align
      'vertical-align      : text-top  ;'+	// cal--currentMonth-vertical-align
      'border-width        : 1px       ;'+
      'border-style        : solid     ;'+
      'border-left-color   : white     ;'+
      'border-top-color    : white     ;'+
      'border-right-color  : black     ;'+
      'border-bottom-color : black     ;'+
      'width               : 14%       ;'+
      'cursor              : hand      ;'
    )
  goStyle['Day'] = StyleInfo.rules[StyleInfo.rules.length - 1].style

  fnLoadCSSDefault('cal--currentMonth-background-color', 'calCurrentMonthBackgroundColor', goStyle['Day'], 'backgroundColor')
  fnLoadCSSDefault('cal--currentMonth-color',            'calCurrentMonthColor',           goStyle['Day'], 'color')
  fnLoadCSSDefault('cal--currentMonth-font-family',      'calCurrentMonthFontFamily',      goStyle['Day'], 'fontFamily')
  fnLoadCSSDefault('cal--currentMonth-font-size',        'calCurrentMonthFontSize',        goStyle['Day'], 'fontSize')
  fnLoadCSSDefault('cal--currentMonth-font-weight',      'calCurrentMonthFontWeight',      goStyle['Day'], 'fontWeight')
  fnLoadCSSDefault('cal--currentMonth-text-align',       'calCurrentMonthTextAlign',       goStyle['Day'], 'textAlign')
  fnLoadCSSDefault('cal--currentMonth-vertical-align',   'calCurrentMonthVerticalAlign',   goStyle['Day'], 'verticalAlign')


  StyleInfo.addRule(   '.DaySelected_' + uniqueID,
      //'background-color    : #7F7F7F  ;'+	// cal--selectedDay-background-color
      'background-color    : #7F7B6E  ;'+	// cal--selectedDay-background-color
      'color               : yellow   ;'+	// cal--selectedDay-color
      'font-family         : Arial    ;'+	// cal--selectedDay-font-family
      'font-size           : 8pt      ;'+	// cal--selectedDay-font-size
      'font-weight         : normal   ;'+	// cal--selectedDay-font-weight
      'text-align          : right    ;'+ 	// cal--selectedMonth-text-align
      'vertical-align      : text-top ;'+	// cal--selectedMonth-vertical-align
      'border-width        : 1px      ;'+
      'border-style        : solid    ;'+
      'border-left-color   : black    ;'+
      'border-top-color    : black    ;'+
      'border-right-color  : #BFBFBF  ;'+
      'border-bottom-color : #BFBFBF  ;'+
      'width               : 14%       ;'+
      'cursor              : hand     ;'
    )
  goStyle['DaySelected'] = StyleInfo.rules[StyleInfo.rules.length - 1].style

  fnLoadCSSDefault('cal--selectedDay-background-color', 'calSelectedDayBackgroundColor', goStyle['DaySelected'], 'backgroundColor')
  fnLoadCSSDefault('cal--selectedDay-color',            'calSelectedDayColor',           goStyle['DaySelected'], 'color')
  fnLoadCSSDefault('cal--selectedDay-font-family',      'calSelectedDayFontFamily',      goStyle['DaySelected'], 'fontFamily')
  fnLoadCSSDefault('cal--selectedDay-font-size',        'calSelectedDayFontSize',        goStyle['DaySelected'], 'fontSize')
  fnLoadCSSDefault('cal--selectedDay-font-weight',      'calSelectedDayFontWeight',      goStyle['DaySelected'], 'fontWeight')
  fnLoadCSSDefault('cal--selectedDay-text-align',       'calSelectedDayTextAlign',       goStyle['DaySelected'], 'textAlign')
  fnLoadCSSDefault('cal--selectedDay-vertical-align',   'calSelectedDayVerticalAlign',   goStyle['DaySelected'], 'verticalAlign')

/*******************************************************/
//Risun added to change rest day to red color. 2000.8.10
/*******************************************************/
  StyleInfo.addRule(   '.DayRest_' + uniqueID,
      //'background-color    : lightgrey ;'+	// cal--currentMonth-background-color
      'background-color    : cornsilk ;'+	// cal--currentMonth-background-color
      'color               : #ff0000   ;'+	// cal--currentMonth-color
      'font-family         : Arial     ;'+	// cal--currentMonth-font-family
      'font-size           : 8pt       ;'+	// cal--currentMonth-font-size
      'font-weight         : normal    ;'+	// cal--currentMonth-font-weight
      'text-align          : right     ;'+ 	// cal--currentMonth-text-align
      'vertical-align      : text-top  ;'+	// cal--currentMonth-vertical-align
      'border-width        : 1px       ;'+
      'border-style        : solid     ;'+
      'border-left-color   : white     ;'+
      'border-top-color    : white     ;'+
      'border-right-color  : black     ;'+
      'border-bottom-color : black     ;'+
      'width               : 14%       ;'+
      'cursor              : hand      ;'
    )
  goStyle['DayRest'] = StyleInfo.rules[StyleInfo.rules.length - 1].style

  fnLoadCSSDefault('cal--restDay-background-color', 'calRestDayBackgroundColor', goStyle['DayRest'], 'backgroundColor')
  fnLoadCSSDefault('cal--restDay-color',            'calRestDayColor',           goStyle['DayRest'], 'color')
  fnLoadCSSDefault('cal--restDay-font-family',      'calRestDayFontFamily',      goStyle['DayRest'], 'fontFamily')
  fnLoadCSSDefault('cal--restDay-font-size',        'calRestDayFontSize',        goStyle['DayRest'], 'fontSize')
  fnLoadCSSDefault('cal--restDay-font-weight',      'calRestDayFontWeight',      goStyle['DayRest'], 'fontWeight')
  fnLoadCSSDefault('cal--restDay-text-align',       'calRestDayTextAlign',       goStyle['DayRest'], 'textAlign')
  fnLoadCSSDefault('cal--restDay-vertical-align',   'calRestDayVerticalAlign',   goStyle['DayRest'], 'verticalAlign')
/****************************************************************/
}

//------------------------------------------------------------------------
//
//  Function:  fnLoadCSSDefault
//
//  Synopsis:  This helper function checks to see if a CSS property
//             extension was used to specify a custom style for the
//             calendar.  If so, the style style object.
//
//  Arguments: sCSSProp        The CSS property extension used by the
//                             page author
//             sScriptProp     The scriptable property name to add to the
//                             style Name of the style rule
//             sStyleRuleProp  Name of the CSS property on the style rule
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnLoadCSSDefault(sCSSProp, sScriptProp, oStyleRule, sStyleRuleProp)
{
  if (element.currentStyle[sCSSProp])
  {
    oStyleRule[sStyleRuleProp] = element.currentStyle[sCSSProp]
  }
  element.style[sScriptProp] = oStyleRule[sStyleRuleProp]
}

//------------------------------------------------------------------------
//
//  Function:  fnGetPropertyDefaults
//
//  Synopsis:  When the for the properties.
//             If so, error checking is performed and the state of the
//             calendar is updated.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetPropertyDefaults()
{
  var x
  var oDate = new Date()

  giDay = oDate.getDate()
  giMonth = oDate.getMonth() + 1
  giYear = oDate.getYear()

  // The JavaScript Date.getYear function returns a 2 digit date representation
  // for dates in the 1900's and a 4 digit date for 2000 and beyond.
  if (giYear < 100) giYear += 1900

  // BUGBUG : Need to fill in day/month/year loading and error checking
  if (element.year)
  {
    if (! isNaN(parseInt(element.year, 10))) giYear = parseInt(element.year, 10)
    if (giYear < giMinYear) giYear = giMinYear
    if (giYear > giMaxYear) giYear = giMaxYear
  }

  fnCheckLeapYear(giYear)

  if (element.month)
  {
    if (! isNaN(parseInt(element.month, 10))) giMonth = parseInt(element.month, 10)
    if (giMonth < 1) giMonth = 1
    if (giMonth > 12) giMonth = 12
  }

  if (element.day)
  {
    if (! isNaN(parseInt(element.day, 10))) giDay = parseInt(element.day, 10)
    if (giDay < 1) giDay = 1
    if (giDay > gaMonthDays[giMonth - 1]) giDay = gaMonthDays[giMonth - 1]
  }

  if (element.monthLength)
  {
    switch (element.monthLength.toLowerCase())
    {
      case 'short' :
        giMonthLength = 0
        break
      case 'long' :
        giMonthLength = 1
        break
    }
  }

  if (element.dayLength)
  {
    switch (element.dayLength.toLowerCase())
    {
      case 'short' :
        giDayLength = 0
        break
      case 'medium' :
        giDayLength = 1
        break
      case 'long' :
        giDayLength = 1
        break
    }
  }

  if (element.firstDay)
  {
    if ((element.firstDay >= 0) && (element.firstDay <= 6))
      giFirstDay = element.firstDay
  }

  if (element.gridCellEffect)
  {
    switch (element.gridCellEffect.toLowerCase())
    {
      case 'raised' :
        giGridCellEffect = 'raised'
        break
      case 'flat' :
        giGridCellEffect = 'flat'
        break
      case 'sunken' :
        giGridCellEffect = 'sunken'
        break
    }
  }

  if (element.gridLinesColor)
    gsGridLinesColor = element.gridLinesColor

  if (element.showDateSelectors)
    gbShowDateSelectors = (element.showDateSelectors) ? true : false

  if (element.showDays)
    gbShowDays = (element.showDays) ? true : false

  if (element.showTitle)
    gbShowTitle = (element.showTitle) ? true : false

  if (element.showHorizontalGrid)
    gbShowHorizontalGrid = (element.showHorizontalGrid) ? true : false

  if (element.showVerticalGrid)
    gbShowVerticalGrid = (element.showVerticalGrid) ? true : false

  if (element.valueIsNull)
    gbValueIsNull = (element.valueIsNull) ? true : false

  if (element.name)
    gsName = element.name

  if (element.readOnly)
    gbReadOnly = (element.readOnly) ? true : false
}

// **********************************************************************
//                       CALENDAR CONTROL FUNCTIONS
// **********************************************************************

//------------------------------------------------------------------------
//
//  Function:  fnSetDate
//
//  Synopsis:  The fnSetDate function is used internally to set the day,
//             month, and year values.
//
//  Arguments: iDay     The new day
//             iMonth   The new month
//             iYear    The new year
//
//  Returns:   none
//
//  Notes:     It does some error checking and some minor performance
//             optimizations if the month & year are not being changed.
//
//------------------------------------------------------------------------

function fnSetDate(iDay, iMonth, iYear)
{
  var bValueChange = false
  if (gbValueIsNull)
  {
    gbValueIsNull = false
    fnFireOnPropertyChange("propertyName", "valueIsNull")
  }

  if (iYear < giMinYear) iYear = giMinYear
  if (iYear > giMaxYear) iYear = giMaxYear
  if (giYear != iYear)
  {
    fnCheckLeapYear(iYear)
  }

  if (iMonth < 1) iMonth = 1
  if (iMonth > 12) iMonth = 12

  if (iDay < 1) iDay = 1
  //Risun changed: 当点在前一个月的日期上时，如果该日期大于当前月的最大日期，以下语句会将其该为当前月的最大日期，正确处理应为上一个月的实际日期。
  //if (iDay > gaMonthDays[giMonth - 1]) iDay = gaMonthDays[giMonth - 1]
  if (iDay > gaMonthDays[iMonth - 1]) iDay = gaMonthDays[iMonth - 1]
  if ((giDay == iDay) && (giMonth == iMonth) && (giYear == iYear))
    return
  else
    bValueChange = true

  if (giDay != iDay)
  {
    giDay = iDay
    fnFireOnPropertyChange("propertyName", "day")
  }

  if ((giYear == iYear) && (giMonth == iMonth))
  {
    //Risun changed to change rest day to red color.
    //goCurrentDayCell.className = 'Day_' + uniqueID
    if ((goCurrentDayCell.cellIndex % 7 == 0) || (goCurrentDayCell.cellIndex % 7 == 6))
		goCurrentDayCell.className = 'DayRest_' + uniqueID
	else
		goCurrentDayCell.className = 'Day_' + uniqueID
    goCurrentDayCell = gaDayCell[giStartDayIndex + iDay - 1]
    goCurrentDayCell.className = 'DaySelected_' + uniqueID
    giDay = iDay
  }
  else
  {

    if (giYear != iYear)
    {
      giYear = iYear
      fnFireOnPropertyChange("propertyName", "year")
      fnUpdateYearSelect()
    }

    if (giMonth != iMonth)
    {
      giMonth = iMonth
      fnFireOnPropertyChange("propertyName", "month")
      fnUpdateMonthSelect()
    }

    fnUpdateTitle()
    fnFillInCells()
  }

  if (bValueChange) fnFireOnPropertyChange("propertyName", "value")
}

//------------------------------------------------------------------------
//
//  Function:  fnUpdateTitle
//
//  Synopsis:  This function updates the title with the currently selected
//             month and year.  The month is displayed in the format
//             specified by the monthLength option
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnUpdateTitle()
{
  var oTitleCell = element.children[0].rows[0].cells[0]
  if (gbShowTitle)
    //Risun changed, Overturn the order of month and year. 2000.8.7
//==============================================
//Risun changed for multi language. 2001.7.20
//----------------------------------------------
    switch (giLanguage)
    {
      case 0://English
        oTitleCell.innerHTML = gaMonthNames_English[giMonthLength][giMonth - 1] + " " + giYear
        break;
      case 1://Simplified Chinese
        oTitleCell.innerHTML = giYear + "\u5e74 " + gaMonthNames_SimChinese[giMonthLength][giMonth - 1]
        break;
      case 2://Traditional Chinese
        oTitleCell.innerHTML = giYear + "\ue703 " + gaMonthNames_TraChinese[giMonthLength][giMonth - 1]
        break;
      default :
        oTitleCell.innerHTML = giYear + "\u5e74 " + gaMonthNames[giMonthLength][giMonth - 1]
     }
//----------------------------------------------
  else
    oTitleCell.innerText = ' '
}

//------------------------------------------------------------------------
//
//  Function:  fnUpdateDayTitles
//
//  Synopsis:  This function updates the titles for the days of the week.
//             They are displayed in the format specified by the dayLength
//             option beginning with the day of the week specified by the
//             firstDay option.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnUpdateDayTitles()
{
  var dayTitleRow = element.children[0].rows[1].cells[0].children[0].rows[0]
  var iCell = 0

  for (i=giFirstDay ; i < 7 ; i++)
  {
//==============================================
//Risun changed for multi language. 2001.7.20
//----------------------------------------------
    switch (giLanguage)
    {
      case 0://English
        goDayTitleRow.cells[iCell++].innerText = gaDayNames_English[giDayLength][i]
        break;
      case 1://Simplified Chinese
        goDayTitleRow.cells[iCell++].innerText = gaDayNames_SimChinese[giDayLength][i]
        break;
      case 2://Traditional Chinese
        goDayTitleRow.cells[iCell++].innerText = gaDayNames_TraChinese[giDayLength][i]
        break;
      default :
        //orginal:
        goDayTitleRow.cells[iCell++].innerText = gaDayNames[giDayLength][i]
    }
//----------------------------------------------
  }

  for (i=0; i < giFirstDay; i++)
  {
//==============================================
//Risun changed for multi language. 2001.7.20
//----------------------------------------------
    switch (giLanguage)
    {
      case 0://English
        goDayTitleRow.cells[iCell++].innerText = gaDayNames_English[giDayLength][i]
        break;
      case 1://Simplified Chinese
        goDayTitleRow.cells[iCell++].innerText = gaDayNames_SimChinese[giDayLength][i]
        break;
      case 2://Traditional Chinese
        goDayTitleRow.cells[iCell++].innerText = gaDayNames_TraChinese[giDayLength][i]
        break;
      default :
        //orginal:
        goDayTitleRow.cells[iCell++].innerText = gaDayNames[giDayLength][i]
    }
//----------------------------------------------

  }
}

//------------------------------------------------------------------------
//
//  Function:  fnUpdateMonthSelect
//
//  Synopsis:  When the month changes, this function is called to select
//             the correct month in the month select control.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnUpdateMonthSelect()
{
  goMonthSelect.options[ giMonth - 1 ].selected = true
}

//------------------------------------------------------------------------
//
//  Function:  fnBuildMonthSelect
//
//  Synopsis:  When the calendar is created, this function inserts the
//             month values into the select control and selects the
//             current month.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnBuildMonthSelect()
{
  var newMonthSelect

  newMonthSelect = element.document.createElement("SELECT")
  goMonthSelect.parentElement.replaceChild(newMonthSelect, goMonthSelect)
  goMonthSelect = newMonthSelect

  for (i=0 ; i < 12; i++)
  {
    e = element.document.createElement("OPTION")
//==============================================
//Risun changed for multi language. 2001.7.20
//----------------------------------------------
    switch (giLanguage)
    {
      case 0://English
        e.text = gaMonthNames_English[giMonthLength][i]
        break;
      case 1://Simplified Chinese
        e.text = gaMonthNames_SimChinese[giMonthLength][i]
        break;
      case 2://Traditional Chinese
        e.text = gaMonthNames_TraChinese[giMonthLength][i]
        break;
      default :
        e.text = gaMonthNames[giMonthLength][i]
     }
    //e.text = gaMonthNames[giMonthLength][i]
//----------------------------------------------
    goMonthSelect.options.add(e)
  }

  goMonthSelect.options[ giMonth - 1 ].selected = true
  goMonthSelect.attachEvent("onchange", fnMonthSelectOnChange)
}

//------------------------------------------------------------------------
//
//  Function:  fnMonthSelectOnChange
//
//  Synopsis:  When the user changes the month using the month select
//             control, this function handles the onSelectChange event
//             to update the date.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnMonthSelectOnChange()
{
  iMonth = goMonthSelect.selectedIndex + 1
  fnSetDate(giDay, iMonth, giYear)
}

//------------------------------------------------------------------------
//
//  Function:  fnUpdateYearSelect
//
//  Synopsis:  When the year changes, this function is called to select
//             the correct year in the year select control.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnUpdateYearSelect()
{
  goYearSelect.options[ giYear - giMinYear ].selected = true
}

//------------------------------------------------------------------------
//
//  Function:  fnBuildYearSelect
//
//  Synopsis:  When the calendar is created, this function inserts the
//             year values into the select control and selects the
//             current year.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnBuildYearSelect()
{
  var newYearSelect
  newYearSelect = element.document.createElement("SELECT")
  //************************************************************
  // 在Windows XP，IE 6.0 环境下，select标签缺省下拉列表的行数会尽量撑满，
  // 最多到屏幕高度，为此将该值在程序中改为定值：
  newYearSelect.size = 1
  //************************************************************
  goYearSelect.parentElement.replaceChild(newYearSelect, goYearSelect)
  goYearSelect = newYearSelect
  for (i=giMinYear; i <= giMaxYear; i++)
  {
    e = element.document.createElement("OPTION")
    e.text = i
    goYearSelect.options.add(e)
  }

  goYearSelect.options[ giYear - giMinYear ].selected = true
  goYearSelect.attachEvent("onchange", fnYearSelectOnChange)
}

//------------------------------------------------------------------------
//
//  Function:  fnYearSelectOnChange
//
//  Synopsis:  When the user changes the year using the year select
//             control, this function handles the onSelectChange event
//             to update the date.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnYearSelectOnChange()
{
  iYear = goYearSelect.selectedIndex + giMinYear
  fnSetDate(giDay, giMonth, iYear)
}

//------------------------------------------------------------------------
//
//  Function:  fnCheckLeapYear
//
//  Synopsis:  When the year changes, this function must be called to
//             ensure that February has the correct count for the number
//             of days.
//
//  Arguments: year
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnCheckLeapYear(iYear)
{
  gaMonthDays[1] = (((!(iYear % 4)) && (iYear % 100) ) || !(iYear % 400)) ? 29 : 28
}

//------------------------------------------------------------------------
//
//  Function:  fnFillInCells
//
//  Synopsis:  This method works through the table and sets the day and
//             style needed.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnFillInCells()
{
  var iDayCell = 0
  var iLastMonthIndex, iNextMonthIndex
  var iLastMonthTotalDays

  var iStartDay

  fnCheckLeapYear(giYear)

  iLastMonthDays = gaMonthDays[ ((giMonth - 1 == 0) ? 12 : giMonth - 1) - 1]
  iNextMonthDays = gaMonthDays[ ((giMonth + 1 == 13) ? 1 : giMonth + 1) - 1]

  iLastMonthYear = (giMonth == 1)  ? giYear - 1 : giYear
  iLastMonth     = (giMonth == 1)  ? 12         : giMonth - 1
  iNextMonthYear = (giMonth == 12) ? giYear + 1 : giYear
  iNextMonth     = (giMonth == 12) ? 1          : giMonth + 1

  var oDate = new Date(giYear, (giMonth - 1), 1)

  iStartDay = oDate.getDay() - giFirstDay
  if (iStartDay < 1) iStartDay += 7
  iStartDay = iLastMonthDays - iStartDay + 1

  for (i = iStartDay ; i <= iLastMonthDays  ; i++ , iDayCell++)
  {
     gaDayCell[iDayCell].innerText = i
     if (gaDayCell[iDayCell].className != 'OffDay_' + uniqueID)
     gaDayCell[iDayCell].className = 'OffDay_' + uniqueID

     gaDayCell[iDayCell].day = i
     gaDayCell[iDayCell].month = iLastMonth
     gaDayCell[iDayCell].year = iLastMonthYear
  }

  giStartDayIndex = iDayCell

  for (i = 1 ; i <= gaMonthDays[giMonth - 1] ; i++, iDayCell++)
  {
     gaDayCell[iDayCell].innerText = i

     if (giDay == i)
     {
       goCurrentDayCell = gaDayCell[iDayCell]
       gaDayCell[iDayCell].className = 'DaySelected_' + uniqueID
     }
     //Risun added to change rest day to red color. 2000.8.7
     else if ((iDayCell % 7 == 6) || (iDayCell % 7 == 0))
     {
		gaDayCell[iDayCell].className = 'DayRest_' + uniqueID
     }
     else
     {
       if (gaDayCell[iDayCell].className != 'Day_' + uniqueID)
         gaDayCell[iDayCell].className = 'Day_' + uniqueID
     }

     gaDayCell[iDayCell].day = i
     gaDayCell[iDayCell].month = giMonth
     gaDayCell[iDayCell].year = giYear
  }

  for (i = 1 ; iDayCell < 42 ; i++, iDayCell++)
  {
     gaDayCell[iDayCell].innerText = i
     if (gaDayCell[iDayCell].className != 'OffDay_' + uniqueID)
       gaDayCell[iDayCell].className = 'OffDay_' + uniqueID

     gaDayCell[iDayCell].day = i
     gaDayCell[iDayCell].month = iNextMonth
     gaDayCell[iDayCell].year = iNextMonthYear
  }
}

// **********************************************************************
//                            EVENT HANDLERS
// **********************************************************************

//------------------------------------------------------------------------
//
//  Function:  fnOnClick
//
//  Synopsis:  When the user clicks on the calendar, change the date if
//             needed
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnOnClick()
{
  var e = window.event.srcElement

  if (e.tagName == "TD")
  {

    if (gbReadOnly || (!e.day)) return  // The calendar is read only
    if ((e.year < giMinYear) || (e.year > giMaxYear)) return
    fnSetDate(e.day, e.month, e.year)
  }
}

//------------------------------------------------------------------------
//
//  Function:  fnOnSelectStart
//
//  Synopsis:  This cancels selection when the user clicks and drags the
//             mouse on the calendar.  It can still be selected if the
//             the SelectStart begins outside this element.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnOnSelectStart()
{
  window.event.returnValue = false
  window.event.cancelBubble = true
}

//------------------------------------------------------------------------
//
//  Function:  fnOnReadyStateChange
//
//  Synopsis:  When the behavior is completely loaded, set the global
//             loading flag to false.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     To improve load time, we do not want the put methods on
//             properties to be called.  We also need to keep events from
//             getting fired while the behavior is loading.
//
//------------------------------------------------------------------------

function fnOnReadyStateChange()
{
  gbLoading = (readyState != "complete")
}

//------------------------------------------------------------------------
//
//  Function:  fnOnPropertyChange
//
//  Synopsis:  When a property changes on the element, this function will
//             check it to see if part of the calendar needs to be changed
//             as a result.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     This is currently only checking extended style
//             properties to alter the calendar style sheet rules.
//
//------------------------------------------------------------------------

function fnOnPropertyChange()
{
  if (window.event.propertyName.substring(0, 5) == 'style')
  {
    switch (window.event.propertyName)
    {
      case 'style.calTitleBackgroundColor' :
        goStyle['WholeCalendar'].backgroundColor = style.calTitleBackgroundColor
        goStyle['Title'].backgroundColor = style.calTitleBackgroundColor
        break
      case 'style.calTitleColor' :
        goStyle['Title'].color = style.calTitleColor
        break
      case 'style.calTitleFontFamily' :
        goStyle['Title'].fontFamily = style.calTitleFontFamily
        break
      case 'style.calTitleFontSize' :
        goStyle['Title'].fontSize = style.calTitleFontSize
        break
      case 'style.calTitleFontWeight' :
        goStyle['Title'].fontWeight = style.calTitleFontWeight
        break
      case 'style.calTitleTextAlign' :
        goStyle['Title'].textAlign = style.calTitleTextAlign
        break

      case 'style.calDayTitleBackgroundColor' :
        goStyle['DayTitle'].backgroundColor = style.calDayTitleBackgroundColor
        break
      case 'style.calDayTitleColor' :
        goStyle['DayTitle'].color = style.calDayTitleColor
        break
      case 'style.calDayTitleFontFamily' :
        goStyle['DayTitle'].fontFamily = style.calDayTitleFontFamily
        break
      case 'style.calDayTitleFontSize' :
        goStyle['DayTitle'].fontSize = style.calDayTitleFontSize
        break
      case 'style.calDayTitleFontWeight' :
        goStyle['DayTitle'].fontWeight = style.calDayTitleFontWeight
        break
      case 'style.calDayTitleTextAlign' :
        goStyle['DayTitle'].textAlign = style.calDayTitleTextAlign
        break

      case 'style.calOffMonthBackgroundColor' :
        goStyle['OffDay'].backgroundColor = style.calOffMonthBackgroundColor
        break
      case 'style.calOffMonthColor' :
        goStyle['OffDay'].color = style.calOffMonthColor
        break
      case 'style.calOffMonthFontFamily' :
        goStyle['OffDay'].fontFamily = style.calOffMonthFontFamily
        break
      case 'style.calOffMonthFontSize' :
        goStyle['OffDay'].fontSize = style.calOffMonthFontSize
        break
      case 'style.calOffMonthFontWeight' :
        goStyle['OffDay'].fontWeight = style.calOffMonthFontWeight
        break
      case 'style.calOffMonthTextAlign' :
        goStyle['OffDay'].textAlign = style.calOffMonthTextAlign
        break
      case 'style.calOffMonthVerticalAlign' :
        goStyle['OffDay'].verticalAlign = style.calOffMonthVerticalAlign
        break

      case 'style.calCurrentMonthBackgroundColor' :
        goStyle['Day'].backgroundColor = style.calCurrentMonthBackgroundColor
        break
      case 'style.calCurrentMonthColor' :
        goStyle['Day'].color = style.calCurrentMonthColor
        break
      case 'style.calCurrentMonthFontFamily' :
        goStyle['Day'].fontFamily = style.calCurrentMonthFontFamily
        break
      case 'style.calCurrentMonthFontSize' :
        goStyle['Day'].fontSize = style.calCurrentMonthFontSize
        break
      case 'style.calCurrentMonthFontWeight' :
        goStyle['Day'].fontWeight = style.calCurrentMonthFontWeight
        break
      case 'style.calCurrentMonthTextAlign' :
        goStyle['Day'].textAlign = style.calCurrentMonthTextAlign
        break
      case 'style.calCurrentMonthVerticalAlign' :
        goStyle['Day'].verticalAlign = style.calCurrentMonthVerticalAlign
        break

      case 'style.calSelectedDayBackgroundColor' :
        goStyle['DaySelected'].backgroundColor = style.calSelectedDayBackgroundColor
        break
      case 'style.calSelectedDayColor' :
        goStyle['DaySelected'].color = style.calSelectedDayColor
        break
      case 'style.calSelectedDayFontFamily' :
        goStyle['DaySelected'].fontFamily = style.calSelectedDayFontFamily
        break
      case 'style.calSelectedDayFontSize' :
        goStyle['DaySelected'].fontSize = style.calSelectedDayFontSize
        break
      case 'style.calSelectedDayFontWeight' :
        goStyle['DaySelected'].fontWeight = style.calSelectedDayFontWeight
        break
      case 'style.calSelectedDayTextAlign' :
        goStyle['DaySelected'].textAlign = style.calSelectedDayTextAlign
        break
      case 'style.calSelectedDayVerticalAlign' :
        goStyle['DaySelected'].verticalAlign = style.calSelectedDayVerticalAlign
        break
    }
  }
}

// **********************************************************************
//                            HELPER FUNCTIONS
// **********************************************************************

//------------------------------------------------------------------------
//
//  Function:  fnFireOnPropertyChange
//
//  Synopsis:
//
//  Arguments:
//
//  Returns:
//
//  Notes:
//
//------------------------------------------------------------------------

function fnFireOnPropertyChange(name1, value1)
{
  var evObj = createEventObject()
  evObj.setAttribute(name1, value1)
  onPropertyChange.fire(evObj)
}

//------------------------------------------------------------------------
//
//  Function:  fnUpdateGridColors
//
//  Synopsis:  This is a helper function for the calendar grid rendering
//             properties.  It handles setting the style rules to create
//             the desired effects.
//
//  Arguments: none
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnUpdateGridColors()
{
  switch (gsGridCellEffect)
  {
    case "raised" :
      goStyle['OffDay'].borderLeftColor = 'white'
      goStyle['OffDay'].borderTopColor = 'white'
      goStyle['OffDay'].borderRightColor = 'black'
      goStyle['OffDay'].borderBottomColor = 'black'

      goStyle['Day'].borderLeftColor = 'white'
      goStyle['Day'].borderTopColor = 'white'
      goStyle['Day'].borderRightColor = 'black'
      goStyle['Day'].borderBottomColor = 'black'

      goStyle['DaySelected'].borderLeftColor = 'white'
      goStyle['DaySelected'].borderTopColor = 'white'
      goStyle['DaySelected'].borderRightColor = 'black'
      goStyle['DaySelected'].borderBottomColor = 'black'

      break
    case "flat" :
      goStyle['OffDay'].borderLeftColor = goStyle['OffDay'].backgroundColor
      goStyle['OffDay'].borderTopColor = goStyle['OffDay'].backgroundColor
      goStyle['OffDay'].borderRightColor = (gbShowVerticalGrid) ? gsGridLinesColor : goStyle['Day'].backgroundColor
      goStyle['OffDay'].borderBottomColor = (gbShowHorizontalGrid) ? gsGridLinesColor : goStyle['Day'].backgroundColor

      goStyle['Day'].borderLeftColor = goStyle['Day'].backgroundColor
      goStyle['Day'].borderTopColor = goStyle['Day'].backgroundColor
      goStyle['Day'].borderRightColor = (gbShowVerticalGrid) ? gsGridLinesColor : goStyle['Day'].backgroundColor
      goStyle['Day'].borderBottomColor = (gbShowHorizontalGrid) ? gsGridLinesColor : goStyle['Day'].backgroundColor

      goStyle['DaySelected'].borderLeftColor = goStyle['DaySelected'].backgroundColor
      goStyle['DaySelected'].borderTopColor = goStyle['DaySelected'].backgroundColor
      goStyle['DaySelected'].borderRightColor = (gbShowVerticalGrid) ? gsGridLinesColor : goStyle['Day'].backgroundColor
      goStyle['DaySelected'].borderBottomColor = (gbShowHorizontalGrid) ? gsGridLinesColor : goStyle['Day'].backgroundColor

      break
    case "sunken" :
      goStyle['OffDay'].borderLeftColor = 'black'
      goStyle['OffDay'].borderTopColor = 'black'
      goStyle['OffDay'].borderRightColor = 'white'
      goStyle['OffDay'].borderBottomColor = 'white'

      goStyle['Day'].borderLeftColor = 'black'
      goStyle['Day'].borderTopColor = 'black'
      goStyle['Day'].borderRightColor = 'white'
      goStyle['Day'].borderBottomColor = 'white'

      goStyle['DaySelected'].borderLeftColor = 'black'
      goStyle['DaySelected'].borderTopColor = 'black'
      goStyle['DaySelected'].borderRightColor = 'white'
      goStyle['DaySelected'].borderBottomColor = 'white'

      break
    default :
      throw 450
  }
}
//------------------------------------------------------------------------
//
//  Function:  fnGetLanguage / fnPutLanguage
//
//  Synopsis:  The language property is used to change the language of
//             char in the calendar grid.
//
//  Arguments: The put method requires a value of 'english', 'simplified',
//             or 'traditional'
//
//  Returns:   The get method will return a value of 'english', 'simplified',
//             or 'traditional'
//
//  Notes:     none
//
//------------------------------------------------------------------------

function fnGetLanguage()
{
  if (giLanguage == 0) return "english"
  if (giLanguage == 1) return "simplified"
  if (giLanguage == 2) return "traditional"
}

function fnPutLanguage(sLanguage)
{
  if (gbLoading) return  // return if the behavior is loading

  switch (sLanguage.toLowerCase())
  {
    case "english" :
      //if (giLanguage == 0) return
      giLanguage = 0
      break;
    case "simplified" :
      //if (giLanguage == 1) return
      giLanguage = 1
      break;
    case "traditional" :
      //if (giLanguage == 2) return
      giLanguage = 2
      break;
    default :
      throw 450
      return
  }
  fnUpdateTitle()
  fnBuildMonthSelect()
  fnUpdateDayTitles()
}
function fnCheckCharset()
{
  var iCharset = -1
  if(typeof(document.parentWindow.document.charset) == "string")
  {
    switch(document.parentWindow.document.charset)
    {
      case "gb2312":
        iCharset = 1
        break
      case "big5":
        iCharset = 2
        break
    case "utf-8":
		//为适应英文版系统作此修改
		iCharset = 0;
        break;
      default:
        iCharset = 1
    }
  }
  else
  {
    iCharset = 0
  }
  return(iCharset)
}

