<?xml version="1.0" encoding="UTF-8"?>
<!--Designed and generated by Altova StyleVision Enterprise Edition 2008 rel. 2 sp2 - see http://www.altova.com/stylevision for more information.-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:output version="1.0" method="xml" encoding="UTF-8" indent="no"/>
	<xsl:param name="SV_OutputFormat" select="'PDF'"/>
	<xsl:param name="ACTUAL_SHIPDATE_MAX" select="'20'"/>
	<xsl:param name="CURRENT_DATE_MAX" select="'20'"/>
	<xsl:param name="CUST_PO_max" select="'30'"/>
	<xsl:param name="DUTY_CODE_MAX" select="'20'"/>
	<xsl:param name="INTL_CARRIER_MAX" select="'20'"/>
	<xsl:param name="PACK_ID_MAX" select="'20'"/>
	<xsl:param name="SHIP_FROM_CITY_MAX" select="'30'"/>
	<xsl:param name="SHIP_FROM_COUNTRY_NAME_MAX" select="'30'"/>
	<xsl:param name="SHIP_FROM_NAME_2_MAX" select="'30'"/>
	<xsl:param name="SHIP_FROM_NAME_MAX" select="'30'"/>
	<xsl:param name="SHIP_FROM_STATE_MAX" select="'3'"/>
	<xsl:param name="SHIP_FROM_STREET_2_MAX" select="'35'"/>
	<xsl:param name="SHIP_FROM_STREET_MAX" select="'35'"/>
	<xsl:param name="SHIP_FROM_ZIP_MAX" select="'10'"/>
	<xsl:param name="SHIP_MODE_MAX" select="'20'"/>
	<xsl:param name="SHIP_TO_ID_MAX" select="'30'"/>
	<xsl:param name="SHIP_TO_STATE_max" select="'3'"/>
	<xsl:param name="SHIP_TO_ZIP_MAX" select="'10'"/>
	<xsl:param name="SHIP_VIA_NAME_3_MAX" select="'35'"/>
	<xsl:param name="SHIP_VIA_STREET_MAX" select="'35'"/>
	<xsl:param name="WAYBILL_NUMBER_MAX" select="'30'"/>
	<xsl:variable name="XML" select="/"/>
	<xsl:variable name="fo:layout-master-set">
		<fo:layout-master-set>
			<fo:simple-page-master master-name="default-page" page-height="8.27in" page-width="11.69in" margin-left=".5cm" margin-right=".5cm">
				<fo:region-body margin-top="6.35cm" margin-bottom="0.5in"/>
				<fo:region-before extent="6.35cm"/>
				<fo:region-after extent="0.5in"/>
			</fo:simple-page-master>
		</fo:layout-master-set>
	</xsl:variable>
	<xsl:template match="/">
		<fo:root>
			<xsl:copy-of select="$fo:layout-master-set"/>
			<fo:page-sequence master-reference="default-page" initial-page-number="1" format="1">
				<xsl:call-template name="headerall"/>
				<xsl:call-template name="footerall"/>
				<fo:flow flow-name="xsl-region-body">
					<fo:block>
						<xsl:for-each select="$XML">
							<xsl:for-each select="MASTER_WAYBILLS">
								<xsl:for-each select="MASTER_WAYBILL">
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="8pt" padding="0pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="57.3%"/>
										<fo:table-column column-width="50%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block>
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
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block>
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
									<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="8pt" padding="0pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="10%"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="15%"/>
										<fo:table-column column-width="15%"/>
										<fo:table-column column-width="15%"/>
										<fo:table-column column-width="15%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row background-color="#787878" color="white">
												<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<fo:block/>
														<fo:inline>
															<xsl:text>No.</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" number-columns-spanned="2" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Description</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Unit Qty</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Pallet Qty</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Net Weight</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Gross Weight</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<xsl:for-each select="HOUSE_WAYBILL">
										<fo:inline-container>
											<fo:block>
												<xsl:text>&#x2029;</xsl:text>
											</fo:block>
										</fo:inline-container>
										<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="8pt" padding="0pt" table-layout="fixed" width="100%">
											<fo:table-column column-width="10%"/>
											<fo:table-column column-width="proportional-column-width(1)"/>
											<fo:table-column column-width="proportional-column-width(1)"/>
											<fo:table-column column-width="15%"/>
											<fo:table-column column-width="15%"/>
											<fo:table-column column-width="15%"/>
											<fo:table-column column-width="15%"/>
											<fo:table-body start-indent="0pt">
												<fo:table-row>
													<fo:table-cell border="none" padding="2pt" text-align="center" display-align="before">
														<fo:block>
															<fo:inline>
																<xsl:value-of select="position()"/>
															</fo:inline>
														</fo:block>
													</fo:table-cell>
													<fo:table-cell border="none" number-columns-spanned="2" padding="2pt" text-align="center" display-align="center">
														<fo:block>
															<fo:inline>
																<xsl:text>Notebook Computer</xsl:text>
															</fo:inline>
															<fo:block/>
															<fo:inline font-family="宋体">
																<xsl:text>笔记本电脑</xsl:text>
															</fo:inline>
															<fo:block/>
															<fo:inline>
																<xsl:text>HS - 84713000</xsl:text>
															</fo:inline>
														</fo:block>
													</fo:table-cell>
													<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
														<fo:block>
															<xsl:for-each select="HAWB_UNIT_QTY">
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
																<fo:inline>
																	<xsl:text> Unit</xsl:text>
																</fo:inline>
															</xsl:for-each>
														</fo:block>
													</fo:table-cell>
													<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
														<fo:block>
															<xsl:for-each select="HAWB_PALLET_QTY">
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
																<fo:inline>
																	<xsl:text> Plt</xsl:text>
																</fo:inline>
															</xsl:for-each>
														</fo:block>
													</fo:table-cell>
													<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
														<fo:block>
															<xsl:for-each select="HAWB_ACT_WEIGHT">
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
																<fo:inline>
																	<xsl:text> KG</xsl:text>
																</fo:inline>
															</xsl:for-each>
														</fo:block>
													</fo:table-cell>
													<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
														<fo:block>
															<xsl:for-each select="HAWB_GROSS_WEIGHT">
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
																<fo:inline>
																	<xsl:text> KG</xsl:text>
																</fo:inline>
															</xsl:for-each>
														</fo:block>
													</fo:table-cell>
												</fo:table-row>
											</fo:table-body>
										</fo:table>
									</xsl:for-each>
									<fo:block/>
									<fo:block text-align="center">
										<fo:leader leader-pattern="rule" rule-thickness="1pt" leader-length="100%"/>
									</fo:block>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="8pt" padding="0pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="10%"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="15%"/>
										<fo:table-column column-width="15%"/>
										<fo:table-column column-width="15%"/>
										<fo:table-column column-width="15%"/>
										<fo:table-body start-indent="0pt">
											<fo:table-row font-weight="bold">
												<fo:table-cell border="none" padding="2pt" display-align="center">
													<fo:block/>
												</fo:table-cell>
												<fo:table-cell border="none" number-columns-spanned="2" padding="2pt" text-align="right" display-align="center">
													<fo:block>
														<fo:inline>
															<xsl:text>Total:</xsl:text>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<xsl:for-each select="MASTER_WAYBILL_UNIT_QTY">
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
															<fo:inline>
																<xsl:text> Unit</xsl:text>
															</fo:inline>
														</xsl:for-each>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<xsl:for-each select="MASTER_WAYBILL_PALLET_QTY">
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
															<fo:inline>
																<xsl:text> Pallet</xsl:text>
															</fo:inline>
														</xsl:for-each>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<xsl:for-each select="MASTER_WAYBILL_ACT_WEIGHT">
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
															<fo:inline>
																<xsl:text> KG</xsl:text>
															</fo:inline>
														</xsl:for-each>
													</fo:block>
												</fo:table-cell>
												<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<xsl:for-each select="MASTER_WAYBILL_GROSS_WEIGHT">
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
															<fo:inline>
																<xsl:text> KG</xsl:text>
															</fo:inline>
														</xsl:for-each>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
								</xsl:for-each>
							</xsl:for-each>
							<fo:inline-container>
								<fo:block>
									<xsl:text>&#x2029;</xsl:text>
								</fo:block>
							</fo:inline-container>
							<fo:table border="none" border-collapse="collapse" border-style="none" font-family="Arial" font-size="8pt" font-style="normal" font-variant="normal" font-weight="normal" padding="0pt" display-align="before" table-layout="fixed" width="100%">
								<fo:table-column column-width="0.5in"/>
								<fo:table-column column-width="1.2in"/>
								<fo:table-column column-width="proportional-column-width(1)"/>
								<fo:table-column column-width="6.25in"/>
								<fo:table-column column-width="proportional-column-width(1)"/>
								<fo:table-body start-indent="0pt">
									<fo:table-row>
										<fo:table-cell border="none" border-style="none" padding="0pt" display-align="before">
											<fo:block/>
										</fo:table-cell>
										<fo:table-cell border="none" border-style="none" padding="0pt" text-align="right" display-align="center">
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
											</fo:block>
										</fo:table-cell>
										<fo:table-cell border="none" border-style="none" padding="0pt" text-align="right" display-align="center">
											<fo:block/>
										</fo:table-cell>
										<fo:table-cell border="none" border-style="none" padding="0pt" display-align="before" text-align="right">
											<fo:block/>
										</fo:table-cell>
										<fo:table-cell border="none" border-style="none" padding="0pt" display-align="before" text-align="right">
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
							<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="8pt" padding="0pt" table-layout="fixed" width="100%">
								<fo:table-column column-width="proportional-column-width(1)"/>
								<fo:table-column column-width="30%"/>
								<fo:table-body start-indent="0pt">
									<fo:table-row>
										<fo:table-cell border="none" padding="2pt" display-align="center">
											<fo:block/>
										</fo:table-cell>
										<fo:table-cell border="none" line-height="0pt" display-align="before" padding="2pt" text-align="left">
											<fo:block>
												<fo:inline>
													<xsl:text>THIS DOCUMENT IS TRUE AND ACCURATE.</xsl:text>
												</fo:inline>
											</fo:block>
										</fo:table-cell>
									</fo:table-row>
									<fo:table-row>
										<fo:table-cell border="none" padding="2pt" display-align="center">
											<fo:block/>
										</fo:table-cell>
										<fo:table-cell border="none" height="15pt" padding="2pt" display-align="center">
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
									</fo:table-row>
									<fo:table-row>
										<fo:table-cell border="none" padding="2pt" height="14" display-align="center">
											<fo:block/>
										</fo:table-cell>
										<fo:table-cell border="none" border-top-color="black" border-top-style="solid" border-top-width="0.5pt" padding="2pt" height="14" display-align="center">
											<fo:block>
												<fo:inline>
													<xsl:text>(Signature)</xsl:text>
												</fo:inline>
												<fo:block/>
												<xsl:for-each select="MASTER_WAYBILLS">
													<xsl:for-each select="MASTER_WAYBILL">
														<xsl:for-each select="SHIP_FROM_NAME_2">
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
													</xsl:for-each>
												</xsl:for-each>
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
							<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="8pt" padding="0pt" table-layout="fixed" width="100%">
								<fo:table-column column-width="proportional-column-width(1)"/>
								<fo:table-column column-width="30%"/>
								<fo:table-body start-indent="0pt">
									<fo:table-row>
										<fo:table-cell border="none" border-collapse="collapse" border-style="none" padding="2pt" display-align="center">
											<fo:block/>
										</fo:table-cell>
										<fo:table-cell border="none" border-style="none" padding="0pt" display-align="center">
											<fo:block>
												<fo:block/>
											</fo:block>
										</fo:table-cell>
									</fo:table-row>
								</fo:table-body>
							</fo:table>
						</xsl:for-each>
					</fo:block>
					<fo:block id="SV_RefID_PageTotal"/>
				</fo:flow>
			</fo:page-sequence>
		</fo:root>
	</xsl:template>
	<xsl:template name="headerall">
		<fo:static-content flow-name="xsl-region-before">
			<fo:block>
				<fo:inline-container>
					<fo:block>
						<xsl:text>&#x2029;</xsl:text>
					</fo:block>
				</fo:inline-container>
				<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="8pt" font-style="normal" font-variant="normal" font-weight="normal" padding="0pt" table-layout="fixed" width="100%">
					<fo:table-column column-width="250"/>
					<fo:table-column column-width="250"/>
					<fo:table-column column-width="125"/>
					<fo:table-column column-width="289"/>
					<fo:table-body start-indent="0pt">
						<fo:table-row>
							<fo:table-cell border="none" padding="2pt" display-align="center">
								<fo:block/>
							</fo:table-cell>
							<fo:table-cell border="none" padding="2pt" display-align="center">
								<fo:block/>
							</fo:table-cell>
							<fo:table-cell border="none" padding="2pt" display-align="center">
								<fo:block/>
							</fo:table-cell>
							<fo:table-cell border="none" padding="2pt" display-align="center">
								<fo:block/>
							</fo:table-cell>
						</fo:table-row>
						<fo:table-row>
							<fo:table-cell border="none" line-height="10pt" display-align="center" padding="2pt" text-align="left">
								<fo:block>
									<fo:external-graphic border=" " content-width="40px">
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
							<fo:table-cell border="none" font-family="Arial" font-size="12pt" font-style="normal" font-variant="normal" font-weight="bold" padding="2pt" text-align="left" display-align="center">
								<fo:block>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:block margin="0pt">
										<fo:block>
											<fo:inline>
												<xsl:text>&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; PACKING LIST</xsl:text>
											</fo:inline>
										</fo:block>
									</fo:block>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="none" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt" text-align="left" display-align="center">
								<fo:block/>
							</fo:table-cell>
							<fo:table-cell border="none" padding="2pt" display-align="center">
								<fo:block>
									<fo:inline>
										<xsl:text>PAGE&#160; </xsl:text>
									</fo:inline>
									<fo:page-number/>
									<fo:inline>
										<xsl:text>&#160; of&#160;&#160;&#160;&#160; </xsl:text>
									</fo:inline>
									<fo:page-number-citation ref-id="SV_RefID_PageTotal"/>
									<fo:block/>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<fo:block margin="0pt">
										<fo:block/>
									</fo:block>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
						<fo:table-row>
							<fo:table-cell border="none" display-align="before" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt">
								<fo:block>
									<fo:block/>
									<fo:inline>
										<xsl:value-of select="$XML/MASTER_WAYBILLS/MASTER_WAYBILL/SHIP_FROM_NAME"/>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:value-of select="$XML/MASTER_WAYBILLS/MASTER_WAYBILL/SHIP_FROM_NAME_2"/>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:value-of select="$XML/MASTER_WAYBILLS/MASTER_WAYBILL/SHIP_FROM_STREET"/>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:value-of select="$XML/MASTER_WAYBILLS/MASTER_WAYBILL/SHIP_FROM_STREET_2"/>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:value-of select="$XML/MASTER_WAYBILLS/MASTER_WAYBILL/SHIP_FROM_CITY"/>
									</fo:inline>
									<fo:inline>
										<xsl:text>, </xsl:text>
									</fo:inline>
									<fo:inline>
										<xsl:value-of select="$XML/MASTER_WAYBILLS/MASTER_WAYBILL/SHIP_FROM_ZIP"/>
									</fo:inline>
									<fo:inline>
										<xsl:text>, </xsl:text>
									</fo:inline>
									<fo:inline>
										<xsl:value-of select="$XML/MASTER_WAYBILLS/MASTER_WAYBILL/SHIP_FROM_COUNTRY_NAME"/>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="none" display-align="before" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt">
								<fo:block>
									<fo:inline>
										<xsl:text>&#160; </xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>&#160; </xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="none" display-align="before" padding="2pt">
								<fo:block>
									<fo:inline>
										<xsl:text> MASTER WAYBILL NUMBER</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>CONTAINER ID</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>SHIP DATE</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="none" display-align="before" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt">
								<fo:block>
									<fo:inline>
										<xsl:text>:&#160; </xsl:text>
									</fo:inline>
									<xsl:for-each select="$XML">
										<xsl:for-each select="MASTER_WAYBILLS">
											<xsl:for-each select="MASTER_WAYBILL">
												<xsl:for-each select="MASTER_WAYBILL_NUMBER">
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
											</xsl:for-each>
										</xsl:for-each>
									</xsl:for-each>
									<fo:block/>
									<fo:inline>
										<xsl:text>:&#160; </xsl:text>
									</fo:inline>
									<xsl:for-each select="$XML">
										<xsl:for-each select="MASTER_WAYBILLS">
											<xsl:for-each select="MASTER_WAYBILL">
												<xsl:for-each select="CONTAINER_ID">
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
											</xsl:for-each>
										</xsl:for-each>
									</xsl:for-each>
									<fo:inline>
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>:&#160; </xsl:text>
									</fo:inline>
									<xsl:for-each select="$XML">
										<xsl:for-each select="MASTER_WAYBILLS">
											<xsl:for-each select="MASTER_WAYBILL">
												<xsl:for-each select="ACTUAL_SHIPDATE">
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
											</xsl:for-each>
										</xsl:for-each>
									</xsl:for-each>
									<fo:block/>
									<fo:inline>
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
						<fo:table-row>
							<fo:table-cell border="none" padding="2pt" display-align="center">
								<fo:block>
									<fo:inline>
										<xsl:text>SHIP TO:</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>Globalink LLP c/o</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>Dead end No. 679</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>PEAK Logistics LLP (Unit А)</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>KZKh, Almaty -1 station, station code 700007</xsl:text>
									</fo:inline>
									<fo:block/>
									<fo:inline>
										<xsl:text>Dead end No. 679</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="none" display-align="before" white-space-collapse="true" linefeed-treatment="treat-as-space" wrap-option="no-wrap" white-space-treatment="ignore-if-surrounding-linefeed" padding="2pt">
								<fo:block/>
							</fo:table-cell>
							<fo:table-cell border="none" number-columns-spanned="2" padding="2pt" display-align="center">
								<fo:block/>
							</fo:table-cell>
						</fo:table-row>
					</fo:table-body>
				</fo:table>
			</fo:block>
		</fo:static-content>
	</xsl:template>
	<xsl:template name="footerall">
		<fo:static-content flow-name="xsl-region-after">
			<fo:block>
				<fo:inline-container>
					<fo:block>
						<xsl:text>&#x2029;</xsl:text>
					</fo:block>
				</fo:inline-container>
				<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="7pt" padding="0pt" table-layout="fixed" width="100%">
					<fo:table-column column-width="15%"/>
					<fo:table-column column-width="60%"/>
					<fo:table-column column-width="150"/>
					<fo:table-body start-indent="0pt">
						<fo:table-row>
							<fo:table-cell border="none" padding="0" height="30" text-align="left" display-align="center">
								<fo:block>
									<fo:inline>
										<xsl:text>EMEARAILKZPK130311</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="none" font-size="smaller" padding="0" text-align="left" display-align="center">
								<fo:block/>
							</fo:table-cell>
							<fo:table-cell border="none" padding="0" height="30" text-align="right" display-align="center">
								<fo:block>
									<fo:inline>
										<xsl:text>consolidation pack list</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
						<fo:table-row>
							<fo:table-cell border="none" padding="0" number-columns-spanned="3" display-align="center">
								<fo:block>
									<fo:block text-align="center">
										<fo:leader top="-37pt" leader-pattern="rule" rule-thickness="1" leader-length="100%" color="black"/>
									</fo:block>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
						<fo:table-row>
							<fo:table-cell border="none" font-size="smaller" padding="0" number-columns-spanned="2" height="1" text-align="left" display-align="center">
								<fo:block/>
							</fo:table-cell>
							<fo:table-cell border="none" font-size="smaller" padding="0" height="1" text-align="right" display-align="center">
								<fo:block/>
							</fo:table-cell>
						</fo:table-row>
					</fo:table-body>
				</fo:table>
			</fo:block>
		</fo:static-content>
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
