using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class SubclassSelectorAttribute : PropertyAttribute
    {
        Type m_type;

        public SubclassSelectorAttribute(System.Type type)
        {
            m_type = type;
        }

        public Type GetFieldType()
        {
            return m_type;
        }
    }
}
