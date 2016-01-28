<?xml version="1.0" encoding="UTF-8"?>
<!--Designed and generated by Altova StyleVision Enterprise Edition 2008 rel. 2 sp2 - see http://www.altova.com/stylevision for more information.-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:output version="1.0" method="xml" encoding="UTF-8" indent="no"/>
	<xsl:param name="SV_OutputFormat" select="'PDF'"/>
	<xsl:param name="CUST_ORD_REF_max" select="'30'"/>
	<xsl:param name="CUST_PO_max" select="'30'"/>
	<xsl:param name="DEST_CODE_max" select="'12'"/>
	<xsl:param name="HP_CUST_PN_max" select="'30'"/>
	<xsl:param name="HP_PN_max" select="'30'"/>
	<xsl:param name="HP_SO_max" select="'30'"/>
	<xsl:param name="INTL_CARRIER_max" select="'30'"/>
	<xsl:param name="ODM_BARCODE1_max" select="'10'"/>
	<xsl:param name="ODM_TEXT1_max" select="'44'"/>
	<xsl:param name="ODM_TEXT2_max" select="'10'"/>
	<xsl:param name="ODM_TEXT3_max" select="'21'"/>
	<xsl:param name="SHIP_FROM_CITY_max" select="'10'"/>
	<xsl:param name="SHIP_FROM_COUNTRY_NAME_max" select="'13'"/>
	<xsl:param name="SHIP_FROM_NAME_2_max" select="'30'"/>
	<xsl:param name="SHIP_FROM_NAME_max" select="'30'"/>
	<xsl:param name="SHIP_FROM_STATE_max" select="'10'"/>
	<xsl:param name="SHIP_FROM_STREET_2_max" select="'35'"/>
	<xsl:param name="SHIP_FROM_STREET_max" select="'35'"/>
	<xsl:param name="SHIP_FROM_ZIP_max" select="'7'"/>
	<xsl:param name="SHIP_TO_CITY_max" select="'30'"/>
	<xsl:param name="SHIP_TO_COUNTRY_CODE_max" select="'12'"/>
	<xsl:param name="SHIP_TO_COUNTRY_NAME_max" select="'15'"/>
	<xsl:param name="SHIP_TO_ID_max" select="'30'"/>
	<xsl:param name="SHIP_TO_NAME_2_max" select="'35'"/>
	<xsl:param name="SHIP_TO_NAME_3_max" select="'35'"/>
	<xsl:param name="SHIP_TO_NAME_max" select="'35'"/>
	<xsl:param name="SHIP_TO_STATE_max" select="'3'"/>
	<xsl:param name="SHIP_TO_STREET_2_max" select="'35'"/>
	<xsl:param name="SHIP_TO_STREET_max" select="'35'"/>
	<xsl:param name="SHIP_TO_ZIP_max" select="'10'"/>
	<xsl:param name="WAYBILL_NUMBER_max" select="'30'"/>
	<xsl:variable name="XML" select="/"/>
	<xsl:variable name="fo:layout-master-set">
		<fo:layout-master-set>
			<fo:simple-page-master master-name="default-page" page-height="15.24cm" page-width="10.16cm" margin-left="0.5cm" margin-right="0.5cm">
				<fo:region-body margin-top="0.1cm" margin-bottom="-0.6cm"/>
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
							<xsl:for-each select="PALLETS">
								<xsl:for-each select="PALLET">
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="10pt" font-weight="bold" line-height="7pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row height="5pt" line-height="5pt" width="100%">
												<fo:table-cell border="none" line-height="5pt" number-columns-spanned="3" padding="2pt" text-align="left" display-align="center">
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
									<fo:table border="none" height="20pt" display-align="before" table-layout="fixed" width="100%" border-spacing="2pt">
										<fo:table-column column-width="45%"/>
										<fo:table-column column-width="55%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row width="100%">
												<fo:table-cell border="none" display-align="before" padding="2pt" text-align="left">
													<fo:block>
														<fo:external-graphic border-collapse="separate" content-height="45pt" display-align="before" content-width="45pt">
															<xsl:attribute name="src">
																<xsl:text>url(</xsl:text>
																<xsl:call-template name="double-backslash">
																	<xsl:with-param name="text">
																		<xsl:value-of select="string(&apos;Image_HPLogo_003_20091106.jpg&apos;)"/>
																	</xsl:with-param>
																	<xsl:with-param name="text-length">
																		<xsl:value-of select="string-length(string(&apos;Image_HPLogo_003_20091106.jpg&apos;))"/>
																	</xsl:with-param>
																</xsl:call-template>
																<xsl:text>)</xsl:text>
															</xsl:attribute>
														</fo:external-graphic>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" border-collapse="collapse" font-family="Arial" font-size="9pt" font-weight="bold" line-height="10pt" display-align="before" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt">
													<fo:block>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_FROM_NAME , 1 , $SHIP_FROM_NAME_max )"/>
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
															<xsl:value-of select="substring( SHIP_FROM_CITY , 1 , $SHIP_FROM_CITY_max )"/>
														</fo:inline>
														<xsl:if test="string-length(SHIP_FROM_STATE) &gt;0">
															<fo:inline>
																<xsl:value-of select="concat( &quot;, &quot;, substring( SHIP_FROM_STATE , 1, $SHIP_FROM_STATE_max ))"/>
															</fo:inline>
														</xsl:if>
														<fo:inline>
															<xsl:value-of select="concat(  &quot; &quot;, substring( SHIP_FROM_ZIP , 1 , $SHIP_FROM_ZIP_max),  &quot; &quot;, substring(SHIP_FROM_COUNTRY_NAME , 1 , $SHIP_FROM_COUNTRY_NAME_max))"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" border-collapse="collapse" border-top="1pt" border-top-color="black" border-top-style="solid" font-family="Arial" font-size="10pt" height="9pt" line-height="10pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row width="100%">
												<fo:table-cell border="none" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>DELIVER TO : </xsl:text>
														</fo:inline>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_TO_ID , 1 , $SHIP_TO_ID_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_TO_NAME , 1 , $SHIP_TO_NAME_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_TO_NAME_2 , 1 , $SHIP_TO_NAME_2_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_TO_NAME_3 , 1 , $SHIP_TO_NAME_3_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_TO_STREET , 1 , $SHIP_TO_STREET_max )"/>
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
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" font-family="Arial" font-size="10pt" height="10pt" line-height="10pt" table-layout="fixed" width="100%" border-spacing="2pt">
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="40%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row width="100%">
												<fo:table-cell border="none" border-collapse="collapse" font-family="Arial" font-size="8pt" font-weight="normal" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-columns-spanned="2" padding="2pt" display-align="before">
													<fo:block>
														<fo:inline>
															<xsl:value-of select="substring( SHIP_TO_COUNTRY_NAME , 1 , $SHIP_TO_COUNTRY_NAME_max )"/>
														</fo:inline>
														<fo:inline>
															<xsl:value-of select="concat(&quot;  &quot;, substring( SHIP_TO_ZIP , 1, $SHIP_TO_ZIP_max ))"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" display-align="center">
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
									<fo:table border="none" border-collapse="collapse" border-top="1pt" border-top-color="black" border-top-style="solid" font-family="Arial" font-size="10pt" line-height="10pt" width="100%" table-layout="fixed">
										<fo:table-column column-width="30%"/>
										<fo:table-column column-width="70%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row line-height="8pt" width="100%">
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block/>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block/>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row width="100%">
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>SO #</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:value-of select="substring( HP_SO , 1 , $HP_SO_max )"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row width="100%">
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>PO #</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:value-of select="substring( CUST_PO , 1 , $CUST_PO_max )"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" border-collapse="collapse" font-size="10pt" line-height="14pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row border="none" font-family="Free 3 of 9 Extended" font-size="22pt" line-height="17pt">
												<fo:table-cell border="none" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="CUST_PO">
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
														<fo:inline>
															<xsl:text>*</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="10pt" line-height="10pt" width="100%" table-layout="fixed">
										<fo:table-column column-width="30%"/>
										<fo:table-column column-width="70%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row line-height="5pt">
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block/>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block/>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Part Number</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:value-of select="substring( HP_PN , 1 , $HP_PN_max )"/>
														</fo:inline>
														<fo:inline>
															<xsl:text>&#160;&#160;&#160;&#160; </xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block/>
												</fo:table-cell>
												<fo:table-cell border="none" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<xsl:for-each select="HP_PN">
															<fo:inline font-family="Free 3 of 9 Extended" font-size="22pt">
																<xsl:text>*</xsl:text>
															</fo:inline>
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="22pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="22pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
															<fo:inline font-family="Free 3 of 9 Extended" font-size="22pt">
																<xsl:text>*</xsl:text>
															</fo:inline>
														</xsl:for-each>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Carrier</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:value-of select="substring( INTL_CARRIER , 1 , $INTL_CARRIER_max )"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>HWB #</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:value-of select="substring( WAYBILL_NUMBER , 1 , $WAYBILL_NUMBER_max )"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Box qty&#160; </xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block>
														<xsl:for-each select="PACK_ID_BOX_QTY_PER_PALLET">
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
														<fo:inline>
															<xsl:text>&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; </xsl:text>
														</fo:inline>
														<xsl:for-each select="PACK_ID_BOX_QTY_PER_PALLET">
															<fo:inline font-family="Free 3 of 9 Extended" font-size="22pt">
																<xsl:text>*</xsl:text>
															</fo:inline>
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="22pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="22pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
															<fo:inline font-family="Free 3 of 9 Extended" font-size="22pt">
																<xsl:text>*</xsl:text>
															</fo:inline>
														</xsl:for-each>
														<fo:inline>
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" border-collapse="collapse" border-top="1pt" border-top-color="black" border-top-style="solid" line-height="14pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row border-top="1pt" border-top-color="black" border-top-style="solid" line-height="10pt">
												<fo:table-cell border="none" font-family="Arial" font-size="10pt" font-weight="bold" padding="2pt" display-align="center">
													<fo:block/>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row border-top="1pt" border-top-color="black" border-top-style="solid" line-height="14pt">
												<fo:table-cell border="none" font-family="Arial" font-size="12pt" font-weight="bold" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Pack ID: </xsl:text>
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
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row line-height="20pt">
												<fo:table-cell border="none" font-family="Free 3 of 9 Extended" font-size="24pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="before">
													<fo:block>
														<fo:inline>
															<xsl:text>*</xsl:text>
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
														<fo:inline>
															<xsl:text>*</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row line-height="10pt">
												<fo:table-cell border="none" font-family="Arial" font-size="10pt" font-weight="bold" padding="2pt" display-align="center">
													<fo:block/>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row line-height="14pt">
												<fo:table-cell border="none" font-family="Arial" font-size="12pt" font-weight="bold" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Pallet ID: </xsl:text>
														</fo:inline>
														<xsl:for-each select="PALLET_ID">
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
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row line-height="20pt">
												<fo:table-cell border="none" font-family="Free 3 of 9 Extended" font-size="24pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="PALLET_ID">
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
														<fo:inline>
															<xsl:text>*</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border-bottom="1pt" border-bottom-color="black" border-bottom-style="solid" border-collapse="collapse" border-style="none" font-family="Arial" font-size="8pt" height="2pt" line-height="2pt" table-layout="fixed" width="100%" border="solid 1pt gray">
										<fo:table-column column-width="30%"/>
										<fo:table-column column-width="70%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row line-height="2pt" width="100%">
												<fo:table-cell border="none" padding="2pt" display-align="before">
													<fo:block/>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block/>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row line-height="10pt" width="100%">
												<fo:table-cell border="none" padding="2pt" display-align="before">
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
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" font-family="Arial" font-size="7pt" padding="2pt" text-align="right" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>GCAGLAPAOA130603</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" table-layout="fixed" width="100%" border-spacing="2pt">
										<fo:table-column column-width="17%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row>
												<fo:table-cell border="none" border-collapse="collapse" padding="2pt" height="1" display-align="center">
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
																		<fo:inline font-family="Arial" font-size="12pt" font-weight="bold">
																			<xsl:value-of select="substring( VALUE , 1 , $ODM_TEXT1_max )"/>
																		</fo:inline>
																	</xsl:if>
																</xsl:for-each>
															</fo:block>
														</fo:block>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<xsl:if test="position()  !=  last()">
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
