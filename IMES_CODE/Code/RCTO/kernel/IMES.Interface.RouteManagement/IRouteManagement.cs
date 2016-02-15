/*
 * Description: Route Management Interface
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-03-19   205033            Create
 * Known issues:
 */
using System;
using System.Collections.Generic;

namespace IMES.Route
{
    /// <summary>
    /// One step in a route
    /// </summary>
    [Serializable]
    public struct StepInfo
    {
        /// <summary>
        /// Condition to transfer from FromStation to ToStation
        /// </summary>
        public string Condition;
        /// <summary>
        /// Station to transfer from
        /// </summary>
        public string FromStation;
        /// <summary>
        /// Station to transfer to
        /// </summary>
        public string ToStation;
    }
    /// <summary>
    /// Route information
    /// </summary>
    [Serializable]
    public struct RouteInfo
    {
        /// <summary>
        /// Route identifier.
        /// Assign it as null or empty for a new route
        /// </summary>
        public string RouteId;
        /// <summary>
        /// Name of route
        /// </summary>
        public string Name;
        /// <summary>
        /// Type of route
        /// </summary>
        public string Type;
        /// <summary>
        /// Route description
        /// </summary>
        public string Description;
        /// <summary>
        /// Possible transition steps in a route
        /// </summary>
        public IList<StepInfo> Steps;
    }
    /// <summary>
    /// Route management interface
    /// </summary>
    public interface IRouteManagement
    {
        /// <summary>
        /// Acquiring route identifier full-list in current system
        /// </summary>
        /// <returns></returns>
        IList<string> GetRouteIdList(string routeType);
        /// <summary>
        /// Get detail information of a route
        /// </summary>
        /// <param name="routeId">route identifier</param>
        /// <returns></returns>
        RouteInfo GetRoute(string routeId);
        /// <summary>
        /// Delete a route from current system, all potential transition steps are deleted as well.
        /// </summary>
        /// <param name="routeId">route identifier</param>
        void DeleteRoute(string routeId, string editor);
        /// <summary>
        /// Save a route into current system. A new route will be created if RouteInfo.RouteId is null or "".
        /// </summary>
        /// <param name="ri">Route details</param>
        /// <param name="editor">editor's signature</param>
        /// <returns>route identifier</returns>
        string SaveRoute(RouteInfo ri, string editor);

        /// <summary>
        /// Read a route attribute
        /// </summary>
        /// <param name="routeId">route identifier</param>
        /// <param name="attrName">attribute name</param>
        /// <returns>attribute value</returns>
        string ReadRouteAttr(string routeId, string attrName);
        /// <summary>
        /// create or update a route attribute
        /// </summary>
        /// <param name="routeId">route identifier</param>
        /// <param name="attrName">attribute name</param>
        /// <param name="attrValue">attribute value</param>
        void WriteRouteAttr(string routeId, string attrName, string attrValue, string editor);
        /// <summary>
        /// Delete a route attribute
        /// </summary>
        /// <param name="routeId">route identifier</param>
        /// <param name="attrName">attribute name</param>
        void DeleteRouteAttr(string routeId, string attrName, string editor);

        /// <summary>
        /// Read a station attribute
        /// </summary>
        /// <param name="stationId">station identifier</param>
        /// <param name="attrName">attribute name</param>
        /// <returns>attribute value</returns>
        string ReadStationAttr(string stationId, string attrName);
        /// <summary>
        /// create or update a station attribute
        /// </summary>
        /// <param name="stationId">station identifier</param>
        /// <param name="attrName">attribute name</param>
        /// <param name="attrValue">attribute value</param>
        void WriteStationAttr(string stationId, string attrName, string attrValue, string editor);
        /// <summary>
        /// Delete a station attribute
        /// </summary>
        /// <param name="routeId">route identifier</param>
        /// <param name="attrName">attribute name</param>
        void DeleteStationAttr(string stationId, string attrName, string editor);

        /// <summary>
        /// Acquire workflow identifier list
        /// </summary>
        /// <returns>workflow identifier list</returns>
        IList<string> GetWorkflowIdList();

        /// <summary>
        /// Read workflow
        /// </summary>
        /// <param name="workflowId">workflow identifier</param>
        /// <param name="rulesContent">rules content</param>
        /// <returns>workflow content</returns>
        string ReadWorkflow(string workflowId, out string rulesContent);

        /// <summary>
        /// Write workflow
        /// </summary>
        /// <param name="workflowId">workflow identifier</param>
        /// <param name="workflowContent">workflow content</param>
        /// <param name="rulesContent">rules content</param>
        void WriteWorkflow(string workflowId, string workflowContent, string rulesContent);

        /// <summary>
        /// Delete workflow
        /// </summary>
        /// <param name="workflowId">workflow file name</param>
        void DeleteWorkflow(string workflowId);

        /// <summary>
        /// Read ruleset
        /// </summary>
        /// <param name="name">ruleset name</param>
        /// <returns></returns>
        string ReadRuleSet(string site, string station, string name);

        /// <summary>
        /// Write ruleset
        /// </summary>
        /// <param name="name">ruleset name</param>
        /// <param name="content">ruleset content</param>
        void WriteRuleSet(string site, string station, string name, string content);

        /// <summary>
        /// Delete ruleset
        /// </summary>
        /// <param name="name">ruleset name</param>
        void DeleteRuleSet(string site, string station, string name);

        /// <summary>
        /// Get Station Name List
        /// </summary>
        /// <returns>station list</returns>
        IList<string> GetStationNameList();

        /// <summary>
        /// Transaction Operations
        /// </summary>
        void Begin();

        /// <summary>
        /// Transaction Operations
        /// </summary>
        void Commit();

        /// <summary>
        /// Transaction Operations
        /// </summary>
        void Rollback();

        /// <summary>
        /// Transaction Operations
        /// </summary>
        void Pause();

        /// <summary>
        /// Transaction Operations
        /// </summary>
        void Resume();

        /// <summary>
        /// Test if Route Management is active
        /// </summary>
        /// <returns></returns>
        bool IsActive();
    }

    public interface IRouteManagementService
    {
        IRouteManagement RouteManager { get; }
    }

    public interface IRemotingUrlService
    {
        string RemotingUrl { get; set; }
    }
}
