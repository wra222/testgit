<?xml version="1.0" encoding="UTF-8"?>
<!--Designed and generated by Altova StyleVision Enterprise Edition 2008 rel. 2 sp2 - see http://www.altova.com/stylevision for more information.-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fo="http://www.w3.org/1999/XSL/Format">
	<xsl:output version="1.0" method="xml" encoding="UTF-8" indent="no"/>
	<xsl:param name="SV_OutputFormat" select="'PDF'"/>
	<xsl:param name="CONSOL_INVOICE_max" select="'30'"/>
	<xsl:param name="HP_PN_max" select="'18'"/>
	<xsl:param name="INTL_CARRIER_max" select="'10'"/>
	<xsl:param name="Num_LineItem_PerPage" select="'24'"/>
	<xsl:param name="Num_LineItem_Shift" select="'9'"/>
	<xsl:param name="ODM_BARCODE1_max" select="'10'"/>
	<xsl:param name="ODM_TEXT1_max" select="'44'"/>
	<xsl:param name="ODM_TEXT2_max" select="'10'"/>
	<xsl:param name="ODM_TEXT3_max" select="'21'"/>
	<xsl:param name="PACK_ID_max" select="'13'"/>
	<xsl:param name="WAYBILL_NUMBER_max" select="'19'"/>
	<xsl:variable name="XML" select="/"/>
	<xsl:variable name="fo:layout-master-set">
		<fo:layout-master-set>
			<fo:simple-page-master master-name="default-page" page-height="15.24cm" page-width="10.16cm" margin-left="0.5cm" margin-right="0.5cm">
				<fo:region-body margin-top="0.4cm" margin-bottom="0.3in"/>
				<fo:region-after extent="0.3in"/>
			</fo:simple-page-master>
		</fo:layout-master-set>
	</xsl:variable>
	<xsl:template match="/">
		<fo:root>
			<xsl:copy-of select="$fo:layout-master-set"/>
			<fo:page-sequence master-reference="default-page" initial-page-number="1" format="1">
				<xsl:call-template name="footerall"/>
				<fo:flow flow-name="xsl-region-body">
					<fo:block border="none">
						<xsl:for-each select="$XML">
							<xsl:for-each select="PALLETS">
								<xsl:for-each select="PALLET">
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
									<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="8pt" font-style="normal" font-variant="normal" font-weight="normal" height="8pt" table-layout="fixed" width="100%">
										<fo:table-column column-width="21%"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-body border="none" font-size="8pt" start-indent="0pt">
											<fo:table-row border="none">
												<fo:table-cell border="none" font-family="Arial" font-size="6pt" number-rows-spanned="3" padding="2pt" text-align="center" display-align="center">
													<fo:block>
														<fo:inline border="none">
															<xsl:text>&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; </xsl:text>
														</fo:inline>
														<fo:external-graphic border="none">
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
												<fo:table-cell border="none" border-bottom-color="black" border-bottom-style="solid" border-bottom-width="1pt" border-left-color="black" border-left-style="solid" border-left-width="1pt" border-right-color="black" border-right-style="solid" border-right-width="1pt" border-top-color="black" border-top-style="solid" border-top-width="1pt" font-size="8pt" font-weight="normal" line-height="8pt" number-columns-spanned="2" padding="2pt" text-align="left" display-align="center">
													<fo:block>
														<fo:inline border="none" font-family="Arial" font-size="8pt" height="8pt" line-height="8pt">
															<xsl:text> Pallet ID: </xsl:text>
														</fo:inline>
														<fo:inline border="none">
															<xsl:value-of select="PALLET_ID"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row border="none">
												<fo:table-cell border="none" border-bottom-color="black" border-bottom-style="solid" border-bottom-width="1pt" border-left-color="black" border-left-style="solid" border-left-width="1pt" border-right-color="black" border-right-style="solid" border-right-width="1pt" font-size="8pt" line-height="8pt" number-columns-spanned="2" padding="2pt" text-align="left" display-align="center">
													<fo:block>
														<fo:inline border="none">
															<xsl:text>AWB: </xsl:text>
														</fo:inline>
														<fo:inline border="none">
															<xsl:value-of select="WAYBILL_NUMBER"/>
														</fo:inline>
													</fo:block>
												</fo:table-cell>
											</fo:table-row>
											<fo:table-row border="none" font-size="8pt">
												<fo:table-cell border="none" border-bottom-color="black" border-bottom-style="solid" border-bottom-width="1pt" border-left-color="black" border-left-style="solid" border-left-width="1pt" border-right-color="black" border-right-style="solid" border-right-width="1pt" font-size="8pt" line-height="8pt" number-columns-spanned="2" padding="2pt" text-align="left" display-align="center">
													<fo:block>
														<fo:inline border="none" border-bottom="none" border-left="none" font-family="Arial" font-size="8pt">
															<xsl:text>Total Box Qty: </xsl:text>
														</fo:inline>
														<fo:inline border="none">
															<xsl:value-of select="PALLET_BOX_QTY"/>
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
									<fo:table border="none" font-size="10pt" height="15pt" line-height="15pt" table-layout="fixed" width="100%" border-spacing="2pt">
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-column column-width="proportional-column-width(1)"/>
										<fo:table-body border="none" start-indent="0pt">
											<fo:table-row border="none">
												<fo:table-cell border="none" font-family="Arial" font-size="7pt" display-align="before" number-columns-spanned="2" padding="2pt" text-align="right">
													<fo:block/>
												</fo:table-cell>
											</fo:table-row>
										</fo:table-body>
									</fo:table>
									<xsl:for-each select="PACK_ID_LINE_ITEM">
										<fo:inline border="none">
											<xsl:text>&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; </xsl:text>
										</fo:inline>
										<fo:inline border="none" font-size="8pt">
											<xsl:text>HP P/N:</xsl:text>
										</fo:inline>
										<fo:inline border="none">
											<xsl:text>&#160;</xsl:text>
										</fo:inline>
										<fo:inline border="none" font-size="8pt">
											<xsl:value-of select="HP_PN"/>
										</fo:inline>
										<fo:inline-container>
											<fo:block>
												<xsl:text>&#x2029;</xsl:text>
											</fo:block>
										</fo:inline-container>
										<fo:table border="none" border-collapse="collapse" font-family="Arial" font-size="10pt" height="10pt" line-height="10pt" margin="0pt" table-layout="fixed" width="100%">
											<fo:table-column column-width="50%"/>
											<fo:table-column column-width="50%"/>
											<fo:table-body border="none" start-indent="0pt">
												<fo:table-row border="none">
													<fo:table-cell border="none" font-family="Arial" font-size="8pt" height="20pt" display-align="before" padding="2pt" text-align="center">
														<fo:block>
															<xsl:for-each select="BOX">
																<xsl:if test="position() mod 2 = 1">
																	<xsl:for-each select="SERIAL_NUM">
																		<fo:inline-container>
																			<fo:block>
																				<xsl:text>&#x2029;</xsl:text>
																			</fo:block>
																		</fo:inline-container>
																		<fo:block border="none" line-height="10pt" margin="0pt">
																			<fo:block>
																				<xsl:variable name="value-of-template">
																					<xsl:apply-templates/>
																				</xsl:variable>
																				<xsl:choose>
																					<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																						<fo:block border="none">
																							<xsl:copy-of select="$value-of-template"/>
																						</fo:block>
																					</xsl:when>
																					<xsl:otherwise>
																						<fo:inline border="none">
																							<xsl:copy-of select="$value-of-template"/>
																						</fo:inline>
																					</xsl:otherwise>
																				</xsl:choose>
																			</fo:block>
																		</fo:block>
																		<fo:inline-container>
																			<fo:block>
																				<xsl:text>&#x2029;</xsl:text>
																			</fo:block>
																		</fo:inline-container>
																		<fo:block border="none" font-family="Free 3 of 9 Extended" font-size="20pt" line-height="20pt" margin-left="(100% - 100%) div 2" margin-right="(100% - 100%) div 2" margin="0pt">
																			<fo:block>
																				<fo:inline border="none">
																					<xsl:text>*</xsl:text>
																				</fo:inline>
																				<xsl:variable name="value-of-template">
																					<xsl:apply-templates/>
																				</xsl:variable>
																				<xsl:choose>
																					<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																						<fo:block border="none">
																							<xsl:copy-of select="$value-of-template"/>
																						</fo:block>
																					</xsl:when>
																					<xsl:otherwise>
																						<fo:inline border="none">
																							<xsl:copy-of select="$value-of-template"/>
																						</fo:inline>
																					</xsl:otherwise>
																				</xsl:choose>
																				<fo:inline border="none">
																					<xsl:text>*</xsl:text>
																				</fo:inline>
																			</fo:block>
																		</fo:block>
																	</xsl:for-each>
																</xsl:if>
															</xsl:for-each>
														</fo:block>
													</fo:table-cell>
													<fo:table-cell border="none" font-family="Arial" font-size="8pt" height="20pt" display-align="before" padding="2pt" text-align="center">
														<fo:block>
															<xsl:for-each select="BOX">
																<xsl:if test="position() mod 2 = 0">
																	<xsl:for-each select="SERIAL_NUM">
																		<fo:inline-container>
																			<fo:block>
																				<xsl:text>&#x2029;</xsl:text>
																			</fo:block>
																		</fo:inline-container>
																		<fo:block border="none" line-height="10pt" margin="0pt">
																			<fo:block>
																				<xsl:variable name="value-of-template">
																					<xsl:apply-templates/>
																				</xsl:variable>
																				<xsl:choose>
																					<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																						<fo:block border="none">
																							<xsl:copy-of select="$value-of-template"/>
																						</fo:block>
																					</xsl:when>
																					<xsl:otherwise>
																						<fo:inline border="none">
																							<xsl:copy-of select="$value-of-template"/>
																						</fo:inline>
																					</xsl:otherwise>
																				</xsl:choose>
																			</fo:block>
																		</fo:block>
																		<fo:inline-container>
																			<fo:block>
																				<xsl:text>&#x2029;</xsl:text>
																			</fo:block>
																		</fo:inline-container>
																		<fo:block border="none" font-family="Free 3 of 9 Extended" font-size="20pt" line-height="20pt" margin-left="(100% - 100%) div 2" margin-right="(100% - 100%) div 2" margin="0pt">
																			<fo:block>
																				<fo:inline border="none">
																					<xsl:text>*</xsl:text>
																				</fo:inline>
																				<xsl:variable name="value-of-template">
																					<xsl:apply-templates/>
																				</xsl:variable>
																				<xsl:choose>
																					<xsl:when test="contains(string($value-of-template),'&#x2029;')">
																						<fo:block border="none">
																							<xsl:copy-of select="$value-of-template"/>
																						</fo:block>
																					</xsl:when>
																					<xsl:otherwise>
																						<fo:inline border="none">
																							<xsl:copy-of select="$value-of-template"/>
																						</fo:inline>
																					</xsl:otherwise>
																				</xsl:choose>
																				<fo:inline border="none">
																					<xsl:text>*</xsl:text>
																				</fo:inline>
																			</fo:block>
																		</fo:block>
																	</xsl:for-each>
																</xsl:if>
															</xsl:for-each>
														</fo:block>
													</fo:table-cell>
												</fo:table-row>
											</fo:table-body>
										</fo:table>
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</fo:block>
					<fo:block id="SV_RefID_PageTotal"/>
				</fo:flow>
			</fo:page-sequence>
		</fo:root>
	</xsl:template>
	<xsl:template name="footerall">
		<fo:static-content flow-name="xsl-region-after">
			<fo:block>
				<fo:inline-container>
					<fo:block>
						<xsl:text>&#x2029;</xsl:text>
					</fo:block>
				</fo:inline-container>
				<fo:table border="none" table-layout="fixed" width="100%" border-spacing="2pt">
					<fo:table-column column-width="proportional-column-width(1)"/>
					<fo:table-column column-width="150"/>
					<fo:table-body border="none" start-indent="0pt">
						<fo:table-row border="none">
							<fo:table-cell border="none" padding="0" number-columns-spanned="2" height="30" text-align="center" display-align="center">
								<fo:block>
									<fo:inline border="none">
										<xsl:text>&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; </xsl:text>
									</fo:inline>
									<fo:page-number border="none" font-size="6pt" height="6pt" line-height="6pt"/>
									<fo:inline border="none" font-size="6pt">
										<xsl:text> of </xsl:text>
									</fo:inline>
									<fo:page-number-citation ref-id="SV_RefID_PageTotal" border="none" font-size="6pt"/>
									<fo:inline border="none" font-size="6pt">
										<xsl:text>&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; NAPLB100813</xsl:text>
									</fo:inline>
									<fo:inline border="none">
										<xsl:text>&#160;</xsl:text>
									</fo:inline>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
						<fo:table-row border="none">
							<fo:table-cell border="none" padding="0" number-columns-spanned="2" display-align="center">
								<fo:block>
									<fo:block text-align="center">
										<fo:leader border="none" top="-37pt" leader-pattern="rule" rule-thickness="1" leader-length="100%" color="black"/>
									</fo:block>
								</fo:block>
							</fo:table-cell>
						</fo:table-row>
						<fo:table-row border="none">
							<fo:table-cell border="none" font-size="10pt" padding="0" text-align="left" display-align="center">
								<fo:block/>
							</fo:table-cell>
							<fo:table-cell border="none" font-size="10pt" padding="0" text-align="right" display-align="center">
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
