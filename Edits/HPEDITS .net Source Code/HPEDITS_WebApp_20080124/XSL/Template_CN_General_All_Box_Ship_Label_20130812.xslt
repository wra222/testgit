<?xml version="1.0" encoding="UTF-8"?>
<!--Designed and generated by Altova StyleVision Enterprise Edition 2008 rel. 2 sp2 - see http://www.altova.com/stylevision for more information.-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:output version="1.0" method="xml" encoding="UTF-8" indent="no"/>
	<xsl:param name="SV_OutputFormat" select="'PDF'"/>
	<xsl:param name="BIG_DEAL_END_USER_max" select="'35'"/>
	<xsl:param name="CUST_PO_max" select="'30'"/>
	<xsl:param name="DEST_CODE_max" select="'12'"/>
	<xsl:param name="DUTY_CODE_max" select="'12'"/>
	<xsl:param name="HP_PN_max" select="'15'"/>
	<xsl:param name="HP_SO_max" select="'21'"/>
	<xsl:param name="INTL_CARRIER_max" select="'30'"/>
	<xsl:param name="LABEL_INSTR_HEAD_max" select="'35'"/>
	<xsl:param name="ODM_BARCODE1_max" select="'10'"/>
	<xsl:param name="ODM_TEXT1_max" select="'60'"/>
	<xsl:param name="ODM_TEXT2_max" select="'12'"/>
	<xsl:param name="ODM_TEXT3_max" select="'21'"/>
	<xsl:param name="PN_max" select="'16'"/>
	<xsl:param name="REG_CARRIER_max" select="'12'"/>
	<xsl:param name="SHIP_FROM_CITY_max" select="'12'"/>
	<xsl:param name="SHIP_FROM_COUNTRY_NAME_max" select="'15'"/>
	<xsl:param name="SHIP_FROM_NAME_2_max" select="'40'"/>
	<xsl:param name="SHIP_FROM_NAME_3_max" select="'35'"/>
	<xsl:param name="SHIP_FROM_NAME_max" select="'30'"/>
	<xsl:param name="SHIP_FROM_STATE_max" select="'8'"/>
	<xsl:param name="SHIP_FROM_STREET_2_max" select="'35'"/>
	<xsl:param name="SHIP_FROM_STREET_max" select="'35'"/>
	<xsl:param name="SHIP_FROM_ZIP_max" select="'10'"/>
	<xsl:param name="SHIP_TO_CITY_max" select="'30'"/>
	<xsl:param name="SHIP_TO_COUNTRY_NAME_max" select="'24'"/>
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
			<fo:simple-page-master master-name="default-page" page-height="55mm" page-width="127mm" margin-left="0.2cm" margin-right="0.2cm">
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
							<xsl:for-each select="BOXES">
								<xsl:for-each select="BOX">
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" border-collapse="collapse" border-top-style="none" font-family="Arial" font-size="6pt" line-height="8pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="37%"/>
										<fo:table-column column-width="55%"/>
										<fo:table-body font-size="6pt" start-indent="0pt">
											<fo:table-row border-collapse="collapse" font-size="6pt" width="100%">
												<fo:table-cell border="none" border-bottom-color="black" border-bottom-style="solid" border-bottom-width="0.5pt" border-left-style="none" font-family="微软雅黑" font-size="6pt" font-weight="normal" line-height="6pt" display-align="before" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-columns-spanned="2" padding="2pt" height="1">
													<fo:block>
														<fo:block/>
														<fo:external-graphic>
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
														<fo:block/>
														<fo:block/>
														<fo:inline>
															<xsl:text>&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; </xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" border-bottom-color="black" border-bottom-style="solid" border-bottom-width="0.5pt" border-left-style="none" font-family="微软雅黑" font-size="6pt" font-weight="normal" line-height="6pt" display-align="before" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" height="1" text-align="left">
													<fo:block>
														<fo:inline font-family="微软雅黑" font-size="6pt" font-weight="bold">
															<xsl:text>起运地</xsl:text>
														</fo:inline>
														<fo:inline font-family="新宋体" font-size="6pt" font-weight="bold">
															<xsl:text> :</xsl:text>
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
															<xsl:value-of select="substring( SHIP_FROM_NAME_3 , 1 , $SHIP_FROM_NAME_3_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline>
															<xsl:value-of select="substring(  SHIP_FROM_STREET  , 1 , $SHIP_FROM_STREET_max )"/>
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
														<fo:inline font-family="微软雅黑" font-weight="normal">
															<xsl:value-of select="concat(  &quot; &quot;, substring( SHIP_FROM_ZIP , 1 , $SHIP_FROM_ZIP_max),  &quot; &quot;, substring(SHIP_FROM_COUNTRY_NAME , 1 , $SHIP_FROM_COUNTRY_NAME_max))"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" border-left-style="none" font-family="微软雅黑" font-size="8pt" font-weight="normal" line-height="8.5pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-rows-spanned="2" padding="2pt" height="1" text-align="right" display-align="before">
													<fo:block>
														<fo:inline>
															<xsl:text>&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; </xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="7pt" font-weight="bold">
															<xsl:value-of select="substring( DEST_CODE , 1 , $DEST_CODE_max )"/>
														</fo:inline>
														<fo:inline>
															<xsl:text>&#160;&#160; </xsl:text>
														</fo:inline>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="26pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="BOX_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="26pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="26pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="26pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="Arial" font-size="6pt" font-weight="normal">
															<xsl:text>BOX ID</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt" font-weight="normal">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="微软雅黑" font-size="6pt" font-weight="normal">
															<xsl:text>箱号</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt" font-weight="normal">
															<xsl:text>: </xsl:text>
														</fo:inline>
														<xsl:for-each select="BOX_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Arial" font-size="6pt" font-weight="normal">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Arial" font-size="6pt" font-weight="normal">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:block/>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="26pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="PACK_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="26pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="26pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="26pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="Arial" font-size="6pt" font-weight="normal">
															<xsl:text>PACK ID</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt" font-weight="normal">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="微软雅黑" font-size="6pt" font-weight="normal">
															<xsl:text>包装编号</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt" font-weight="normal">
															<xsl:text>: </xsl:text>
														</fo:inline>
														<xsl:for-each select="PACK_ID">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Arial" font-size="6pt" font-weight="normal">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Arial" font-size="6pt" font-weight="normal">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:block/>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="26pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="WAYBILL_NUMBER">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="26pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="26pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="26pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="Arial" font-size="7pt" font-weight="normal">
															<xsl:text>AWB / SHIP REF:</xsl:text>
														</fo:inline>
														<fo:inline font-size="7pt" font-weight="normal">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="7pt" font-weight="normal">
															<xsl:value-of select="substring( WAYBILL_NUMBER , 1 , $WAYBILL_NUMBER_max )"/>
														</fo:inline>
														<fo:block/>
														<xsl:if test="string-length(LABEL_INSTR_HEAD) &gt;0">
															<fo:inline font-family="Arial" font-size="7pt" font-weight="normal">
																<xsl:text>End User:</xsl:text>
															</fo:inline>
															<fo:inline font-family="微软雅黑" font-size="7pt" font-weight="normal">
																<xsl:text>&#160;</xsl:text>
															</fo:inline>
															<fo:inline font-family="微软雅黑" font-size="7pt" font-weight="normal" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed">
																<xsl:value-of select="substring( LABEL_INSTR_HEAD , 1 , $LABEL_INSTR_HEAD_max )"/>
															</fo:inline>
														</xsl:if>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row border-collapse="collapse" line-height="normal" width="100%">
												<fo:table-cell border="none" border-collapse="collapse" font-family="微软雅黑" font-size="7.5pt" font-weight="normal" line-height="8pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-columns-spanned="3" padding="2pt" height="1" display-align="before">
													<fo:block>
														<fo:inline font-family="微软雅黑" font-weight="bold">
															<xsl:text>收货方/运到</xsl:text>
														</fo:inline>
														<fo:inline font-family="新宋体" font-weight="bold">
															<xsl:text> :</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="7.5pt">
															<xsl:value-of select="substring(SHIP_TO_ID , 1 , $SHIP_TO_ID_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="微软雅黑" font-size="7.5pt">
															<xsl:value-of select="substring(  SHIP_TO_NAME  , 1 , $SHIP_TO_NAME_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="微软雅黑" font-size="7.5pt">
															<xsl:value-of select="substring(  SHIP_TO_NAME_2  , 1 , $SHIP_TO_NAME_2_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="微软雅黑" font-size="7.5pt">
															<xsl:value-of select="substring(  SHIP_TO_NAME_3  , 1 , $SHIP_TO_NAME_3_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="微软雅黑" font-size="7.5pt">
															<xsl:value-of select="substring(  SHIP_TO_STREET , 1 , $SHIP_TO_STREET_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="微软雅黑" font-size="7.5pt">
															<xsl:value-of select="substring( SHIP_TO_STREET_2 , 1 , $SHIP_TO_STREET_2_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="微软雅黑" font-size="7.5pt">
															<xsl:value-of select="substring( SHIP_TO_CITY , 1 , $SHIP_TO_CITY_max )"/>
														</fo:inline>
														<xsl:if test="string-length(SHIP_TO_STATE) &gt;0">
															<fo:inline font-family="微软雅黑" font-size="7.5pt">
																<xsl:value-of select="concat(&quot;, &quot;, substring( SHIP_TO_STATE , 1, $SHIP_TO_STATE_max ))"/>
															</fo:inline>
														</xsl:if>
														<fo:block/>
														<fo:inline font-family="微软雅黑" font-size="7.5pt">
															<xsl:value-of select="substring( SHIP_TO_COUNTRY_NAME , 1 , $SHIP_TO_COUNTRY_NAME_max )"/>
														</fo:inline>
														<fo:inline font-family="微软雅黑" font-size="7.5pt">
															<xsl:value-of select="concat(&quot;  &quot;, substring( SHIP_TO_ZIP , 1, $SHIP_TO_ZIP_max ))"/>
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
									<fo:table border="none" border-collapse="collapse" border-top-style="none" line-height="3pt" width="100%" table-layout="fixed">
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="50%"/>
										<fo:table-body line-height="3pt" start-indent="0pt">
											<fo:table-row>
												<fo:table-cell border="none" font-family="新宋体" font-size="7pt" line-height="5pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-columns-spanned="5" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="20pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
														<xsl:for-each select="CUST_PO">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-family="Free 3 of 9 Extended" font-size="20pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-family="Free 3 of 9 Extended" font-size="20pt">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-family="Free 3 of 9 Extended" font-size="20pt">
															<xsl:text>*</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row>
												<fo:table-cell border="none" font-family="Arial" font-size="7pt" font-weight="bold" line-height="6pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-columns-spanned="2" padding="2pt" display-align="center">
													<fo:block>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>PO# </xsl:text>
														</fo:inline>
														<fo:inline font-family="微软雅黑" font-size="6pt">
															<xsl:text>客户订单号</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt">
															<xsl:text>:</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:value-of select="substring( CUST_PO , 1 , $CUST_PO_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>SO#</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="微软雅黑" font-size="6pt">
															<xsl:text>销售合同号</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt">
															<xsl:text>:</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:value-of select="substring( HP_SO , 1 , $HP_SO_max )"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" font-family="Arial" font-size="7pt" line-height="6pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-columns-spanned="3" padding="2pt" text-align="right" display-align="center">
													<fo:block>
														<fo:inline font-family="Arial" font-size="6pt" font-weight="normal">
															<xsl:text>P/N#</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt" font-weight="normal">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="微软雅黑" font-size="6pt" font-weight="normal">
															<xsl:text>产品货号</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt" font-weight="normal">
															<xsl:text>:</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt" font-weight="normal">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:value-of select="substring( HP_PN , 1 , $HP_PN_max )"/>
														</fo:inline>
														<fo:block/>
														<fo:inline font-size="6pt" font-weight="normal">
															<xsl:text>Box Qty:1 [Box/Total: </xsl:text>
														</fo:inline>
														<xsl:for-each select="BOX_SEQUENCE">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-size="6pt" font-weight="normal">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-size="6pt" font-weight="normal">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-size="6pt" font-weight="normal">
															<xsl:text> of </xsl:text>
														</fo:inline>
														<xsl:for-each select="PACK_ID_BOX_QTY">
															<xsl:variable name="value-of-template">
																<xsl:apply-templates/>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																	<fo:block font-size="6pt" font-weight="normal">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:block>
																</xsl:when>
																<xsl:otherwise>
																	<fo:inline font-size="6pt" font-weight="normal">
																		<xsl:copy-of select="$value-of-template"/>
																	</fo:inline>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
														<fo:inline font-size="6pt" font-weight="normal">
															<xsl:text>]</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row line-height="3pt" width="100%">
												<fo:table-cell border="none" font-family="Arial" font-size="7pt" font-weight="bold" line-height="2pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" number-columns-spanned="4" padding="2pt" display-align="before">
													<fo:block>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>Carrier</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="微软雅黑" font-size="6pt">
															<xsl:text>承运商</xsl:text>
														</fo:inline>
														<fo:inline font-size="6pt">
															<xsl:text>:</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>&#160;</xsl:text>
														</fo:inline>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:value-of select="substring( INTL_CARRIER , 1 , $INTL_CARRIER_max )"/>
														</fo:inline>
														<xsl:if test="string-length(REG_CARRIER) &gt;0">
															<fo:inline font-family="Arial" font-size="6pt">
																<xsl:value-of select="concat(&quot; / &quot;, substring( REG_CARRIER , 1, $REG_CARRIER_max ))"/>
															</fo:inline>
														</xsl:if>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" font-family="Arial" font-size="8pt" padding="2pt" text-align="right" display-align="center">
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
									<fo:table border="none" border-collapse="collapse" border-top-style="none" height="10pt" line-height="10pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="10%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row line-height="4pt">
												<fo:table-cell border="none" border-top-color="transparent" border-top-style="none" line-height="2pt" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" display-align="before">
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
																		<fo:inline font-family="Arial" font-size="6pt" font-weight="bold">
																			<xsl:value-of select="substring( VALUE , 1 , $ODM_TEXT1_max )"/>
																		</fo:inline>
																	</xsl:if>
																</xsl:for-each>
															</fo:block>
														</fo:block>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" border-bottom-style="none" border-left-style="none" font-family="新宋体" font-size="6pt" font-weight="bold" line-height="2pt" display-align="before" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" text-align="right">
													<fo:block>
														<fo:inline font-family="Arial" font-size="6pt">
															<xsl:text>CNBOX130812</xsl:text>
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
