<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:output version="1.0" method="xml" encoding="UTF-8" indent="no"/>
	<xsl:param name="SV_OutputFormat" select="'PDF'"/>
	<xsl:param name="SUB_REGION_max" select="'12'"/>
	<xsl:param name="WAYBILL_NUMBER_max" select="'20'"/>
	<xsl:param name="INTL_CARRIER_max" select="'18'"/>
	<xsl:param name="DEST_CODE_max" select="'12'"/>
	<xsl:param name="HP_SO_max" select="'20'"/>
	<xsl:param name="PACK_ID_max" select="'15'"/>
	<xsl:variable name="XML1" select="/"/>
	<xsl:variable name="fo:layout-master-set">
		<fo:layout-master-set>
			<fo:simple-page-master master-name="default-page" page-height="11.69in" page-width="8.27in" margin-left="0.45cm" margin-right="0.45cm">
				<fo:region-body margin-top="1.0cm" margin-bottom="1.0cm"/>
				<fo:region-before extent="1.0cm"/>
				<fo:region-after extent="1.0cm"/>
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
						<xsl:for-each select="$XML1">
							<xsl:for-each select="WAYBILL_ADDITION">
								<xsl:for-each select="SUBREGION">
									<xsl:if test="position() &gt; 1">
										<fo:block break-after="page">
											<fo:leader leader-pattern="space"/>
										</fo:block>
									</xsl:if>
									<fo:inline-container>
										<fo:block>
											<xsl:text>&#x2029;</xsl:text>
										</fo:block>
									</fo:inline-container>
									<xsl:if test="DESTINATION_CODE">
										<fo:table border="1pt" border-collapse="collapse" font-family="Arial" font-size="8pt" margin="0pt" table-layout="fixed" width="100%">
											<fo:table-column column-width="20%"/>
											<fo:table-column column-width="20%"/>
											<fo:table-column column-width="proportional-column-width(1)"/>
											<fo:table-column column-width="15%"/>
											<fo:table-column column-width="17%"/>
											<fo:table-header start-indent="0pt">
												<fo:table-row>
													<fo:table-cell border="none" number-columns-spanned="5" padding="2pt" text-align="left" display-align="center">
														<fo:block>
															<fo:inline-container>
																<fo:block>
																	<xsl:text>&#x2029;</xsl:text>
																</fo:block>
															</fo:inline-container>
															<fo:table border="1pt" border-collapse="collapse" font-family="Arial" font-size="8pt" font-style="normal" font-variant="normal" font-weight="normal" margin="0pt" linefeed-treatment="treat-as-space" white-space-collapse="true" white-space-treatment="ignore-if-surrounding-linefeed" wrap-option="no-wrap" table-layout="fixed" width="100%">
																<fo:table-column column-width="25%"/>
																<fo:table-column column-width="44%"/>
																<fo:table-column column-width="9%"/>
																<fo:table-column column-width="8%"/>
																<fo:table-column column-width="14%"/>
																<fo:table-body start-indent="0pt">
																	<fo:table-row>
																		<fo:table-cell border="none" padding="2pt" text-align="left" display-align="center">
																			<fo:block>
																				<fo:inline>
																					<xsl:text>BOX SORT CODE:&#160; </xsl:text>
																				</fo:inline>
																				<fo:inline>
																					<xsl:value-of select="substring(   SUB_REGION   , 1 , $SUB_REGION_max )"/>
																				</fo:inline>
																			</fo:block>
																		</fo:table-cell>
																		<fo:table-cell border="none" padding="2pt" text-align="left" display-align="center">
																			<fo:block>
																				<fo:inline>
																					<xsl:text>INTERNATIONAL CARRIER / FF:&#160; </xsl:text>
																				</fo:inline>
																				<fo:inline>
																					<xsl:value-of select="substring(   INTL_CARRIER   , 1 , $INTL_CARRIER_max )"/>
																				</fo:inline>
																			</fo:block>
																		</fo:table-cell>
																		<fo:table-cell border="none" padding="2pt" text-align="left" display-align="center">
																			<fo:block>
																				<fo:inline>
																					<xsl:text>SHIP DATE:</xsl:text>
																				</fo:inline>
																			</fo:block>
																		</fo:table-cell>
																		<fo:table-cell border="none" padding="2pt" text-align="left" display-align="center">
																			<fo:block>
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
																			</fo:block>
																		</fo:table-cell>
																		<fo:table-cell border="none" padding="2pt" text-align="left" display-align="center">
																			<fo:block/>
																		</fo:table-cell>
																	</fo:table-row>
																	<fo:table-row>
																		<fo:table-cell border="none" padding="2pt" text-align="left" display-align="center">
																			<fo:block>
																				<fo:inline>
																					<xsl:text>AIR WAYBILL #:&#160; </xsl:text>
																				</fo:inline>
																				<fo:inline>
																					<xsl:value-of select="substring(   WAYBILL_NUMBER   , 1 , $WAYBILL_NUMBER_max )"/>
																				</fo:inline>
																			</fo:block>
																		</fo:table-cell>
																		<fo:table-cell border="none" padding="2pt" display-align="center">
																			<fo:block/>
																		</fo:table-cell>
																		<fo:table-cell border="none" padding="2pt" text-align="left" display-align="center">
																			<fo:block>
																				<fo:inline>
																					<xsl:text>DATE:</xsl:text>
																				</fo:inline>
																			</fo:block>
																		</fo:table-cell>
																		<fo:table-cell border="none" padding="2pt" text-align="left" display-align="center">
																			<fo:block>
																				<xsl:for-each select="CURRENT_DATE">
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
																		<fo:table-cell border="none" padding="2pt" text-align="right" display-align="center">
																			<fo:block>
																				<fo:inline>
																					<xsl:text>&#160;&#160;&#160;&#160;&#160; </xsl:text>
																				</fo:inline>
																			</fo:block>
																		</fo:table-cell>
																	</fo:table-row>
																</fo:table-body>
															</fo:table>
														</fo:block>
													</fo:table-cell>
												</fo:table-row>
												<fo:table-row>
													<fo:table-cell border="none" border-bottom-color="black" border-bottom-style="double" border-bottom-width="thick" border-top-color="black" border-top-style="double" border-top-width="thick" padding="2pt" text-align="left" display-align="center">
														<fo:block>
															<fo:inline>
																<xsl:text>BOX ID</xsl:text>
															</fo:inline>
														</fo:block>
													</fo:table-cell>
													<fo:table-cell border="none" border-bottom-color="black" border-bottom-style="double" border-bottom-width="thick" border-top-color="black" border-top-style="double" border-top-width="thick" padding="2pt" text-align="left" display-align="center">
														<fo:block>
															<fo:inline>
																<xsl:text>PACK ID</xsl:text>
															</fo:inline>
														</fo:block>
													</fo:table-cell>
													<fo:table-cell border="none" border-bottom-color="black" border-bottom-style="double" border-bottom-width="thick" border-top-color="black" border-top-style="double" border-top-width="thick" padding="2pt" text-align="left" display-align="center">
														<fo:block>
															<fo:inline>
																<xsl:text>HP SO #</xsl:text>
															</fo:inline>
														</fo:block>
													</fo:table-cell>
													<fo:table-cell border="none" border-bottom-color="black" border-bottom-style="double" border-bottom-width="thick" border-top-color="black" border-top-style="double" border-top-width="thick" padding="2pt" text-align="center" display-align="center">
														<fo:block>
															<fo:inline>
																<xsl:text>DUTY CODE</xsl:text>
															</fo:inline>
														</fo:block>
													</fo:table-cell>
													<fo:table-cell border="none" border-bottom-color="black" border-bottom-style="double" border-bottom-width="thick" border-top-color="black" border-top-style="double" border-top-width="thick" padding="2pt" text-align="center" display-align="center">
														<fo:block>
															<fo:inline>
																<xsl:text>BOX WEIGHT</xsl:text>
															</fo:inline>
															<fo:block/>
															<fo:inline>
																<xsl:text>KG</xsl:text>
															</fo:inline>
														</fo:block>
													</fo:table-cell>
												</fo:table-row>
											</fo:table-header>
											<fo:table-body start-indent="0pt">
												<xsl:for-each select="DESTINATION_CODE">
													<fo:table-row>
														<fo:table-cell border="none" number-columns-spanned="2" padding="2pt" display-align="after">
															<fo:block>
																<fo:inline>
																	<xsl:text>DESTINATION CODE:&#160; </xsl:text>
																</fo:inline>
																<fo:inline>
																	<xsl:value-of select="substring( DEST_CODE , 1 , $DEST_CODE_max )"/>
																</fo:inline>
															</fo:block>
														</fo:table-cell>
														<fo:table-cell border="none" padding="2pt" display-align="center">
															<fo:block/>
														</fo:table-cell>
														<fo:table-cell border="none" number-columns-spanned="2" padding="2pt" display-align="center">
															<fo:block/>
														</fo:table-cell>
													</fo:table-row>
													<fo:table-row>
														<fo:table-cell border="none" number-columns-spanned="5" padding="2pt" display-align="center">
															<fo:block>
																<fo:inline-container>
																	<fo:block>
																		<xsl:text>&#x2029;</xsl:text>
																	</fo:block>
																</fo:inline-container>
																<xsl:if test="BOX">
																	<fo:table border="1pt" border-collapse="collapse" font-family="Arial" font-size="8pt" margin="0pt" table-layout="fixed" width="100%">
																		<fo:table-column column-width="20%"/>
																		<fo:table-column column-width="20%"/>
																		<fo:table-column column-width="proportional-column-width(1)"/>
																		<fo:table-column column-width="15%"/>
																		<fo:table-column column-width="15%"/>
																		<fo:table-header start-indent="0pt">
																			<fo:table-row height="0px" line-height="0px">
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
																				<fo:table-cell border="none" padding="2pt" display-align="center">
																					<fo:block/>
																				</fo:table-cell>
																			</fo:table-row>
																		</fo:table-header>
																		<fo:table-body start-indent="0pt">
																			<xsl:for-each select="BOX">
																				<fo:table-row>
																					<fo:table-cell border="none" padding="2pt" display-align="center">
																						<fo:block>
																							<xsl:for-each select="BOX_ID">
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
																					<fo:table-cell border="none" padding="2pt" display-align="center">
																						<fo:block>
																							<fo:inline>
																								<xsl:value-of select="substring( PACK_ID , 1 , $PACK_ID_max )"/>
																							</fo:inline>
																						</fo:block>
																					</fo:table-cell>
																					<fo:table-cell border="none" padding="2pt" display-align="center">
																						<fo:block>
																							<fo:inline>
																								<xsl:value-of select="substring( HP_SO , 1 , $HP_SO_max )"/>
																							</fo:inline>
																						</fo:block>
																					</fo:table-cell>
																					<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
																						<fo:block>
																							<fo:inline>
																								<xsl:value-of select="../../DUTY_CODE"/>
																							</fo:inline>
																						</fo:block>
																					</fo:table-cell>
																					<fo:table-cell border="none" padding="2pt" text-align="center" display-align="center">
																						<fo:block>
																							<xsl:for-each select="BOX_WEIGHT">
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
																			</xsl:for-each>
																		</fo:table-body>
																	</fo:table>
																</xsl:if>
															</fo:block>
														</fo:table-cell>
													</fo:table-row>
													<fo:table-row>
														<fo:table-cell border="none" border-bottom="1pt" border-bottom-color="black" border-bottom-style="solid" number-columns-spanned="2" padding="2pt" display-align="before">
															<fo:block>
																<fo:inline>
																	<xsl:text>DESTINATION CODE:&#160; </xsl:text>
																</fo:inline>
																<fo:inline>
																	<xsl:value-of select="substring( DEST_CODE , 1 , $DEST_CODE_max )"/>
																</fo:inline>
															</fo:block>
														</fo:table-cell>
														<fo:table-cell border="none" border-bottom="1pt" border-bottom-color="black" border-bottom-style="solid" padding="2pt" display-align="center">
															<fo:block>
																<fo:inline font-weight="bold">
																	<xsl:text>BOX SUB TOTAL:&#160; </xsl:text>
																</fo:inline>
																<xsl:for-each select="DEST_CODE_BOX_QTY">
																	<xsl:variable name="value-of-template">
																		<xsl:apply-templates/>
																	</xsl:variable>
																	<xsl:choose>
																		<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																			<fo:block font-weight="bold">
																				<xsl:copy-of select="$value-of-template"/>
																			</fo:block>
																		</xsl:when>
																		<xsl:otherwise>
																			<fo:inline font-weight="bold">
																				<xsl:copy-of select="$value-of-template"/>
																			</fo:inline>
																		</xsl:otherwise>
																	</xsl:choose>
																</xsl:for-each>
															</fo:block>
														</fo:table-cell>
														<fo:table-cell border="none" border-bottom="1pt" border-bottom-color="black" border-bottom-style="solid" number-columns-spanned="2" padding="2pt" text-align="right" display-align="center">
															<fo:block>
																<fo:inline font-weight="bold">
																	<xsl:text>NET WEIGHT:&#160;&#160; </xsl:text>
																</fo:inline>
																<xsl:for-each select="DEST_CODE_EXTD_BOX_WGT">
																	<xsl:variable name="value-of-template">
																		<xsl:apply-templates/>
																	</xsl:variable>
																	<xsl:choose>
																		<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																			<fo:block font-weight="bold">
																				<xsl:copy-of select="$value-of-template"/>
																			</fo:block>
																		</xsl:when>
																		<xsl:otherwise>
																			<fo:inline font-weight="bold">
																				<xsl:copy-of select="$value-of-template"/>
																			</fo:inline>
																		</xsl:otherwise>
																	</xsl:choose>
																</xsl:for-each>
															</fo:block>
														</fo:table-cell>
													</fo:table-row>
												</xsl:for-each>
											</fo:table-body>
										</fo:table>
									</xsl:if>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
						<fo:inline-container>
							<fo:block>
								<xsl:text>&#x2029;</xsl:text>
							</fo:block>
						</fo:inline-container>
						<fo:table border="1pt" border-collapse="collapse" font-family="Arial" font-size="8pt" line-height=".25pt" margin="0pt" padding="none" table-layout="fixed" width="100%">
							<fo:table-column column-width="proportional-column-width(1)"/>
							<fo:table-body start-indent="0pt">
								<fo:table-row>
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
						<fo:table border="1pt" border-collapse="collapse" border-top="1pt" border-top-color="black" border-top-style="double" border-top-width="thick" font-family="Arial" font-size="8pt" font-style="normal" font-variant="normal" font-weight="normal" height="14pt" line-height="14pt" margin="0pt" table-layout="fixed" width="100%">
							<fo:table-column column-width="20%"/>
							<fo:table-column column-width="40%"/>
							<fo:table-column column-width="15%"/>
							<fo:table-column column-width="25%"/>
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
									<fo:table-cell border="none" padding="2pt" display-align="center">
										<fo:block>
											<fo:inline>
												<xsl:text>TOTAL WEIGHT :&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; </xsl:text>
											</fo:inline>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell border="none" padding="2pt" display-align="center">
										<fo:block>
											<fo:inline>
												<xsl:value-of select="$XML1/WAYBILL_ADDITION/SUBREGION/HAWB_ACT_WEIGHT"/>
											</fo:inline>
											<fo:inline>
												<xsl:text> KG</xsl:text>
											</fo:inline>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell border="none" display-align="after" padding="2pt" text-align="right">
										<fo:block>
											<fo:inline>
												<xsl:text>DRIVER NAME:&#160;&#160;&#160;&#160;&#160; </xsl:text>
											</fo:inline>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell border="none" border-bottom="1pt" border-bottom-color="black" border-bottom-style="solid" padding="2pt" display-align="center">
										<fo:block/>
									</fo:table-cell>
								</fo:table-row>
								<fo:table-row>
									<fo:table-cell border="none" padding="2pt" display-align="center">
										<fo:block>
											<fo:inline>
												<xsl:text>TOTAL PALLETS :&#160;&#160;&#160;&#160;&#160;&#160;&#160; </xsl:text>
											</fo:inline>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell border="none" padding="2pt" display-align="center">
										<fo:block>
											<fo:inline>
												<xsl:value-of select="$XML1/WAYBILL_ADDITION/SUBREGION/HAWB_PALLET_QTY"/>
											</fo:inline>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell border="none" display-align="after" padding="2pt" text-align="right">
										<fo:block>
											<fo:inline>
												<xsl:text>SIGNATURE:&#160;&#160;&#160;&#160;&#160; </xsl:text>
											</fo:inline>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell border="none" border-bottom="1pt" border-bottom-color="black" border-bottom-style="solid" padding="2pt" display-align="center">
										<fo:block/>
									</fo:table-cell>
								</fo:table-row>
								<fo:table-row>
									<fo:table-cell border="none" padding="2pt" display-align="center">
										<fo:block>
											<fo:inline>
												<xsl:text>TOTAL BOXES :&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; </xsl:text>
											</fo:inline>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell border="none" padding="2pt" display-align="center">
										<fo:block>
											<fo:inline>
												<xsl:value-of select="$XML1/WAYBILL_ADDITION/SUBREGION/HAWB_BOX_QTY"/>
											</fo:inline>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell border="none" display-align="after" padding="2pt" text-align="right">
										<fo:block>
											<fo:inline>
												<xsl:text>TIME:&#160;&#160;&#160;&#160;&#160; </xsl:text>
											</fo:inline>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell border="none" border-bottom="1pt" border-bottom-color="black" border-bottom-style="solid" padding="2pt" display-align="center">
										<fo:block/>
									</fo:table-cell>
								</fo:table-row>
							</fo:table-body>
						</fo:table>
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
				<fo:table border="1pt" border-collapse="collapse" font-family="Arial" font-size="12pt" font-style="normal" font-variant="normal" font-weight="bold" margin="0pt" table-layout="fixed" width="100%">
					<fo:table-column column-width="proportional-column-width(1)"/>
					<fo:table-column column-width="30%"/>
					<fo:table-body start-indent="0pt">
						<fo:table-row>
							<fo:table-cell border="none" font-size="smaller" padding="0" text-align="left" display-align="center">
								<fo:block>
									<fo:inline>
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="none" font-size="8pt" padding="0" text-align="right" display-align="center">
								<fo:block>
									<fo:inline>
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
						<fo:table-row>
							<fo:table-cell border="none" font-size="smaller" padding="0" text-align="left" display-align="center">
								<fo:block>
									<fo:inline>
										<xsl:text>SCANNING LIST</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="none" font-family="Arial" font-size="8pt" font-style="normal" font-variant="normal" font-weight="normal" padding="0" text-align="right" display-align="center">
								<fo:block>
									<fo:inline>
										<xsl:text>Page:&#160;&#160;&#160;&#160; </xsl:text>
									</fo:inline>
									<fo:page-number/>
									<fo:inline>
										<xsl:text> of </xsl:text>
									</fo:inline>
									<fo:page-number-citation ref-id="SV_RefID_PageTotal"/>
								</fo:block>
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
				<fo:table border="1pt" border-collapse="collapse" font-family="Arial" font-size="7pt" margin="0pt" table-layout="fixed" width="100%">
					<fo:table-column column-width="proportional-column-width(1)"/>
					<fo:table-column column-width="150"/>
					<fo:table-body start-indent="0pt">
						<fo:table-row>
							<fo:table-cell border="none" padding="0" height="30" text-align="left" display-align="center">
								<fo:block>
									<fo:inline>
										<xsl:text>EGLAWAY080905</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="none" padding="0" height="30" text-align="right" display-align="center">
								<fo:block/>
							</fo:table-cell>
						</fo:table-row>
						<fo:table-row>
							<fo:table-cell border="none" padding="0" number-columns-spanned="2" display-align="center">
								<fo:block>
									<fo:block text-align="center">
										<fo:leader top="-37pt" leader-pattern="rule" rule-thickness="1" leader-length="100%" color="black"/>
									</fo:block>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
						<fo:table-row>
							<fo:table-cell border="none" font-size="smaller" padding="0" text-align="left" display-align="center">
								<fo:block>
									<fo:inline font-weight="bold">
										<xsl:text>Document: C:\Documents and Settings\david_riffel\My Documents\AllmyESG2\Active Projects\Direct Ship Stuff\b-EDITS Docs in HVLS SP\Waybill Addition\Template_EMEA_General_All_Waybill_Addition_20080609.sps</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
							<fo:table-cell border="none" font-size="smaller" padding="0" text-align="right" display-align="center">
								<fo:block>
									<fo:inline font-weight="bold">
										<xsl:text>Page: </xsl:text>
									</fo:inline>
									<fo:page-number font-weight="bold"/>
								</fo:block>
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
