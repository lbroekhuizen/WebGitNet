﻿//-----------------------------------------------------------------------
// <copyright file="BreadCrumbTrail.cs" company="(none)">
//  Copyright © 2011 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace WebGitNet.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class BreadCrumbTrail : IEnumerable<BreadCrumbTrail.BreadCrumb>
    {
        private readonly LinkedList<BreadCrumb> breadCrumbs = new LinkedList<BreadCrumb>();

        public void Append(string controller, string action, string name, object routeValues = null)
        {
            this.breadCrumbs.AddLast(new BreadCrumb(name, controller, action, routeValues));
        }

        public void Append<TSource>(string controller, string action, IEnumerable<TSource> crumbSource, Func<TSource, string> nameSelector, Func<TSource, object> routeValuesSelector)
        {
            this.Append(controller, action, crumbSource, nameSelector, (item, name) => routeValuesSelector(item));
        }

        public void Append<TSource>(string controller, string action, IEnumerable<TSource> crumbSource, Func<TSource, string> nameSelector, Func<TSource, string, object> routeValuesSelector)
        {
            foreach (var crumb in crumbSource)
            {
                var name = nameSelector(crumb);
                var routeValues = routeValuesSelector(crumb, name);

                this.Append(controller, action, name, routeValues);
            }
        }

        public IEnumerator<BreadCrumbTrail.BreadCrumb> GetEnumerator()
        {
            return this.breadCrumbs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public sealed class BreadCrumb
        {
            private readonly string name;
            private readonly string controller;
            private readonly string action;
            private readonly object routeValues;

            public BreadCrumb(string name, string controller, string action, object routeValues)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }

                this.name = name;

                if (string.IsNullOrEmpty(controller))
                {
                    throw new ArgumentNullException("controller");
                }

                this.controller = controller;

                if (string.IsNullOrEmpty(action))
                {
                    throw new ArgumentNullException("action");
                }

                this.action = action;

                this.routeValues = routeValues;
            }

            public string Name
            {
                get
                {
                    return this.name;
                }
            }

            public string Controller
            {
                get
                {
                    return this.controller;
                }
            }

            public string Action
            {
                get
                {
                    return this.action;
                }
            }

            public object RouteValues
            {
                get
                {
                    return this.routeValues;
                }
            }
        }
    }
}
