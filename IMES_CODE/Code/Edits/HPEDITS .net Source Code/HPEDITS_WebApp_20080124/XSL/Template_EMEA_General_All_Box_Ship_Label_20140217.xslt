<?xml version="1.0" encoding="UTF-8"?>
<!--Designed and generated by Altova StyleVision Enterprise Edition 2008 rel. 2 sp2 - see http://www.altova.com/stylevision for more information.-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:output version="1.0" method="xml" encoding="UTF-8" indent="no"/>
	<xsl:param name="SV_OutputFormat" select="'PDF'"/>
	<xsl:param name="CUST_PO_max" select="'30'"/>
	<xsl:param name="DEST_CODE_max" select="'12'"/>
	<xsl:param name="DUTY_CODE_max" select="'12'"/>
	<xsl:param name="HP_PN_max" select="'21'"/>
	<xsl:param name="HP_SO_max" select="'21'"/>
	<xsl:param name="INTL_CARRIER_max" select="'30'"/>
	<xsl:param name="ODM_BARCODE1_max" select="'15'"/>
	<xsl:param name="ODM_TEXT1_max" select="'60'"/>
	<xsl:param name="PN_max" select="'21'"/>
	<xsl:param name="REG_CARRIER_max" select="'12'"/>
	<xsl:param name="SHIP_FROM_CITY_max" select="'12'"/>
	<xsl:param name="SHIP_FROM_COUNTRY_NAME_max" select="'15'"/>
	<xsl:param name="SHIP_FROM_NAME_2_max" select="'40'"/>
	<xsl:param name="SHIP_FROM_NAME_max" select="'30'"/>
	<xsl:param name="SHIP_FROM_STATE_max" select="'8'"/>
	<xsl:param name="SHIP_FROM_STREET_2_max" select="'35'"/>
	<xsl:param name="SHIP_FROM_STREET_max" select="'35'"/>
	<xsl:param name="SHIP_FROM_ZIP_max" select="'10'"/>
	<xsl:param name="SHIP_TO_CITY_max" select="'30'"/>
	<xsl:param name="SHIP_TO_COUNTRY_NAME_MAX" select="'24'"/>
	<xsl:param name="SHIP_TO_ID_max" select="'18'"/>
	<xsl:param name="SHIP_TO_NAME_2_max" select="'35'"/>
	<xsl:param name="SHIP_TO_NAME_3_max" select="'35'"/>
	<xsl:param name="SHIP_TO_NAME_max" select="'35'"/>
	<xsl:param name="SHIP_TO_STATE_max" select="'16'"/>
	<xsl:param name="SHIP_TO_STREET_2_max" select="'35'"/>
	<xsl:param name="SHIP_TO_STREET_max" select="'35'"/>
	<xsl:param name="SHIP_TO_ZIP_max" select="'15'"/>
	<xsl:param name="STCC_max" select="'15'"/>
	<xsl:param name="SUB_REGION_max" select="'12'"/>
	<xsl:param name="WAYBILL_NUMBER_max" select="'30'"/>
	<xsl:variable name="XML" select="/"/>
	<xsl:variable name="fo:layout-master-set">
		<fo:layout-master-set>
			<fo:simple-page-master master-name="default-page" page-height="55mm" page-width="127mm" margin-left="0.5cm" margin-right="0.5cm">
				<fo:region-body margin-top="0cm" margin-bottom="-0.6cm"/>
			</fo:simple-page-master>
		</fo:layout-master-set>
	</xsl:variable>
	<xsl:template match="/">
		<fo:root>
			<xsl:copy-of select="$fo:layout-master-set"/>
			<fo:page-sequence master-reference="default-page" initial-page-number="1" format="1">
				<fo:flow flow-name="xsl-region-body">
					<fo:block>
						<xsl:for-each select="$XML">
							<xsl:for-each select="BOXES">
								<xsl:for-each select="BOX">
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" border-collapse="collapse" border-top-style="none" font-family="Arial" font-weight="normal" line-height="7pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="10%"/>
										<fo:table-column column-width="40%"/>
										<fo:table-column column-width="10%"/>
										<fo:table-column column-width="40%"/>
										<fo:table-body font-weight="normal" start-indent="0pt">
											<fo:table-row border-collapse="collapse" width="100%">
												<fo:table-cell border="none" border-bottom-style="none" border-collapse="collapse" font-family="Arial" font-size="6pt" font-weight="bold" line-height="5pt" display-align="center" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="0" height="1" text-align="left">
													<fo:block>
														<fo:inline>
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline-container>
															<fo:block>
																<xsl:text>&#x2029;</xsl:text>
															</fo:block>
														</fo:inline-container>
														<fo:block margin="0pt">
															<fo:block>
																<fo:inline>
																	<xsl:text>&#160;</xsl:text>
																</fo:inline>
															</fo:block>
														</fo:block>
														<fo:inline font-size="10pt">
															<xsl:value-of select="substring(SUB_REGION , 1 , $SUB_REGION_max)"/>
														</fo:inline>
														<fo:inline>
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline-container>
															<fo:block>
																<xsl:text>&#x2029;</xsl:text>
															</fo:block>
														</fo:inline-container>
														<fo:block margin="0pt">
															<fo:block>
																<fo:inline>
																	<xsl:text>&#160;</xsl:text>
																</fo:inline>
															</fo:block>
														</fo:block>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>PACK ID: </xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="7pt">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<xsl:for-each select="PACK_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block>
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline>
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:block/>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="18pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="PACK_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="18pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="18pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="18pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" border-bottom-style="none" border-collapse="collapse" font-family="Arial" font-size="6pt" font-weight="bold" line-height="6pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="0" height="1" text-align="left" display-align="before">
													<fo:block>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="30pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="BOX_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="30pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="30pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="30pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="Arial" font-size="8pt">
															<xsl:text>&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; BOX ID</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial">
															<xsl:text>:</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="8pt">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<xsl:for-each select="BOX_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-size="8pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-size="8pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" border-left-style="none" font-family="Arial" font-size="6pt" font-weight="bold" display-align="before" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="0" height="1" text-align="left">
													<fo:block>
														<fo:block/>
														<fo:inline font-family="Arial" font-size="7pt" font-weight="bold">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline>
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" border-bottom-style="none" border-collapse="collapse" font-family="Arial" font-size="5pt" font-weight="bold" line-height="4.5pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="0" height="1" text-align="right" display-align="before">
													<fo:block>
														<fo:inline-container>
															<fo:block>
																<xsl:text>&#x2029;</xsl:text>
															</fo:block>
														</fo:inline-container>
														<fo:block margin="0pt">
															<fo:block>
																<fo:inline>
																	<xsl:text>&#160;</xsl:text>
																</fo:inline>
															</fo:block>
														</fo:block>
														<fo:inline font-family="Arial" font-weight="bold">
															<xsl:text>SHIP FROM:&#160; </xsl:text>
														</fo:inline>
														<fo:inline>
															<xsl:value-of select="substring(  SHIP_FROM_NAME  , 1 , $SHIP_FROM_NAME_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_FROM_NAME_2 , 1 , $SHIP_FROM_NAME_2_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_FROM_STREET , 1 , $SHIP_FROM_STREET_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_FROM_STREET_2 , 1 , $SHIP_FROM_STREET_2_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="concat(substring(  SHIP_FROM_CITY  , 1 , $SHIP_FROM_CITY_max ),&quot;, &quot;,  substring(  SHIP_FROM_STATE  , 1 , $SHIP_FROM_STATE_max ), &quot; &quot;, substring( SHIP_FROM_ZIP , 1 , $SHIP_FROM_ZIP_max), &quot; &quot;, substring( SHIP_FROM_COUNTRY_NAME , 1 , $SHIP_FROM_COUNTRY_NAME_max))"/>
														</fo:inline>
														<fo:inline-container>
															<fo:block>
																<xsl:text>&#x2029;</xsl:text>
															</fo:block>
														</fo:inline-container>
														<fo:block margin="0pt">
															<fo:block>
																<fo:inline font-size="6pt">
																	<xsl:text>&#160;</xsl:text>
																</fo:inline>
															</fo:block>
														</fo:block>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>Product number: </xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:value-of select="substring( HP_PN , 1 , $HP_PN_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="Arial" font-size="6pt" font-weight="bold">
															<xsl:text>AWB: </xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt" font-weight="bold">
															<xsl:value-of select="substring( WAYBILL_NUMBER , 1 , $WAYBILL_NUMBER_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>Box Qty: 1 [Box of Total: </xsl:text>
														</fo:inline>
														<xsl:for-each select="BOX_SEQUENCE">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Arial" font-size="6pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Arial" font-size="6pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text> of&#160; </xsl:text>
														</fo:inline>
														<xsl:for-each select="PACK_ID_BOX_QTY">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Arial" font-size="6pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Arial" font-size="6pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>]</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row border-collapse="collapse" width="100%">
												<fo:table-cell border="none" border-collapse="collapse" font-family="Arial" font-size="10pt" font-weight="bold" line-height="8pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-columns-spanned="2" padding="0" height="1" display-align="before">
													<fo:block>
														<fo:inline>
															<xsl:text>SHIP TO: </xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:value-of select="substring(  SHIP_TO_ID  , 1 , $SHIP_TO_ID_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring(  SHIP_TO_NAME  , 1 , $SHIP_TO_NAME_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring(  SHIP_TO_NAME_2  , 1 , $SHIP_TO_NAME_2_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring(  SHIP_TO_NAME_3  , 1 , $SHIP_TO_NAME_3_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring(  SHIP_TO_STREET , 1 , $SHIP_TO_STREET_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_TO_STREET_2 , 1 , $SHIP_TO_STREET_2_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_TO_CITY , 1 , $SHIP_TO_CITY_max )"/>
														</fo:inline>
														<xsl:if test="string-length(SHIP_TO_STATE) &gt;0">
															<fo:inline>
																<xsl:value-of select="concat(&quot;, &quot;, substring( SHIP_TO_STATE , 1, $SHIP_TO_STATE_max ))"/>
															</fo:inline>
														</xsl:if>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_TO_COUNTRY_NAME , 1 , $SHIP_TO_COUNTRY_NAME_MAX)"/>
														</fo:inline>
														<fo:inline>
															<xsl:text>&#160; </xsl:text>
														</fo:inline>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_TO_ZIP , 1 , $SHIP_TO_ZIP_max)"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" border-left-style="none" font-family="Arial" font-size="8pt" font-weight="normal" line-height="7.5pt" display-align="auto" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-columns-spanned="2" number-rows-spanned="2" padding="0" height="1" text-align="right">
													<fo:block>
														<fo:inline-container>
															<fo:block>
																<xsl:text>&#x2029;</xsl:text>
															</fo:block>
														</fo:inline-container>
														<fo:block margin="0pt">
															<fo:block>
																<fo:inline>
																	<xsl:text>&#160;</xsl:text>
																</fo:inline>
															</fo:block>
														</fo:block>
														<fo:inline>
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-size="9pt" font-weight="bold">
															<xsl:value-of select="substring( DUTY_CODE , 1 , $DUTY_CODE_max )"/>
														</fo:inline>
														<fo:inline font-size="9pt">
															<xsl:text>/</xsl:text>
														</fo:inline>
														<fo:inline font-size="9pt" font-weight="bold">
															<xsl:value-of select="substring( REG_CARRIER , 1 , $REG_CARRIER_max )"/>
														</fo:inline>
														<fo:inline font-size="9pt">
															<xsl:text>/</xsl:text>
														</fo:inline>
														<fo:inline font-size="9pt" font-weight="bold">
															<xsl:value-of select="substring( DEST_CODE , 1 , $DEST_CODE_max )"/>
														</fo:inline>
														<fo:inline-container>
															<fo:block>
																<xsl:text>&#x2029;</xsl:text>
															</fo:block>
														</fo:inline-container>
														<fo:block margin="0pt">
															<fo:block>
																<fo:inline>
																	<xsl:text>&#160;</xsl:text>
																</fo:inline>
															</fo:block>
														</fo:block>
														<fo:inline font-family="Arial" font-size="10pt" font-weight="bold" line-height="11pt">
															<xsl:text>PACK ID:&#160; </xsl:text>
														</fo:inline>
														<xsl:for-each select="PACK_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-size="10pt" font-weight="bold" line-height="11pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-size="10pt" font-weight="bold" line-height="11pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline>
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="30pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="PACK_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="30pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="30pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="30pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<fo:inline>
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline-container>
															<fo:block>
																<xsl:text>&#x2029;</xsl:text>
															</fo:block>
														</fo:inline-container>
														<fo:block margin="0pt">
															<fo:block>
																<fo:inline font-weight="bold">
																	<xsl:text>&#160;</xsl:text>
																</fo:inline>
															</fo:block>
														</fo:block>
														<fo:inline font-size="6pt" font-weight="bold">
															<xsl:text>Sales order #: </xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt" font-weight="bold">
															<xsl:value-of select="substring( HP_SO , 1 , $HP_SO_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-size="6pt" font-weight="bold">
															<xsl:text>Carrier: </xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt" font-weight="bold">
															<xsl:value-of select="substring( INTL_CARRIER , 1 , $INTL_CARRIER_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-size="6pt" font-weight="bold">
															<xsl:text>PO: </xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt" font-weight="bold">
															<xsl:value-of select="substring( CUST_PO , 1 , $CUST_PO_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="20pt" line-height="10pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="CUST_PO">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="20pt" line-height="10pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="20pt" line-height="10pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="20pt" line-height="10pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row border-collapse="collapse" width="100%">
												<fo:table-cell border="none" font-family="Arial" font-size="8pt" line-height="7pt" display-align="before" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="0" height="1">
													<fo:block>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="30pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="BOX_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="30pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="30pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="30pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="8pt" font-weight="bold">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-weight="bold" display-align="auto">
															<xsl:text>BOX ID: </xsl:text>
														</fo:inline>
														<xsl:for-each select="BOX_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-weight="bold" display-align="auto">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-weight="bold" display-align="auto">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:block/>
														<fo:inline-container>
															<fo:block>
																<xsl:text>&#x2029;</xsl:text>
															</fo:block>
														</fo:inline-container>
														<fo:block margin="0pt">
															<fo:block>
																<fo:inline>
																	<xsl:text>&#160;</xsl:text>
																</fo:inline>
															</fo:block>
														</fo:block>
														<fo:inline font-size="10pt" font-weight="bold">
															<xsl:value-of select="substring(SUB_REGION , 1 , $SUB_REGION_max)"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" font-family="Arial" font-size="7pt" line-height="5.5pt" display-align="text-top" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="0" height="1">
													<fo:block/>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" border-collapse="collapse" margin="none" padding="none" table-layout="fixed" width="100%">
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="50%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row margin="none" padding="none">
												<fo:table-cell border="none" border-collapse="collapse" border-top-style="none" line-height="0.5pt" margin="none" padding="none" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-columns-spanned="3" height="1" display-align="center">
													<fo:block>
														<fo:inline-container>
															<fo:block>
																<xsl:text>&#x2029;</xsl:text>
															</fo:block>
														</fo:inline-container>
														<fo:block white-space="pre" white-space-collapse="false" margin="0pt">
															<fo:block>
																<xsl:for-each select="UDF_HEADER">
																	<xsl:if test="./KEY = &apos;ODM_TEXT1&apos; and string-length(./VALUE)&gt;0">
																		<fo:inline font-family="Arial" font-size="5.5pt" font-weight="bold">
																			<xsl:value-of select="substring( VALUE , 1 , $ODM_TEXT1_max )"/>
																		</fo:inline>
																	</xsl:if>
																</xsl:for-each>
															</fo:block>
														</fo:block>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" border-collapse="collapse" border-top-style="none" font-weight="normal" line-height="0.5pt" margin="none" padding="none" display-align="text-top" height="1" text-align="right">
													<fo:block>
														<fo:inline font-family="Arial" font-size="5pt" font-weight="bold">
															<xsl:text>EMEABOX140217</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<xsl:if test="position() != last()">
										<fo:block break-after="page">
											<fo:leader leader-pattern="space"/>
										</fo:block>
									</xsl:if>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</fo:block>
					<fo:block id="SV_RefID_PageTotal"/>
				</fo:flow>
			</fo:page-sequence>
		</fo:root>
	</xsl:template>
	<xsl:template name="double-backslash">
		<xsl:param name="text"/>
		<xsl:param name="text-length"/>
		<xsl:variable name="text-after-bs" select="substring-after($text, '\')"/>
		<xsl:variable name="text-after-bs-length" select="string-length($text-after-bs)"/>
		<xsl:choose>
			<xsl:when test="$text-after-bs-length = 0">
				<xsl:choose>
					<xsl:when test="substring($text, $text-length) = '\'">
						<xsl:value-of select="concat(substring($text,1,$text-length - 1), '\\')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$text"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat(substring($text,1,$text-length - $text-after-bs-length - 1), '\\')"/>
				<xsl:call-template name="double-backslash">
					<xsl:with-param name="text" select="$text-after-bs"/>
					<xsl:with-param name="text-length" select="$text-after-bs-length"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
