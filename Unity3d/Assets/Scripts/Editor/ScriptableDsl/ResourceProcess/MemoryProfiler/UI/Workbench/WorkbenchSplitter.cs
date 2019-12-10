using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
#endif

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal class WorkbenchSplitter : VisualElement
    {
        public VisualElement LeftPane { get; private set; }
        public VisualElement RightPane { get; private set; }

        public event Action<float> LeftPaneWidthChanged = delegate {};

        VisualElement m_DragLine;

        public WorkbenchSplitter(float initialWorkbenchWidth = 200)
        {
            style.flexGrow = 1;
            style.flexDirection = FlexDirection.Row;

            LeftPane = new VisualElement();
            LeftPane.name = "splitterLeftPane";

            LeftPane.style.width = initialWorkbenchWidth;
            Add(LeftPane);

            var dragLineAnchor = new VisualElement();
            dragLineAnchor.name = "splitterDraglineAnchor";
            Add(dragLineAnchor);

            m_DragLine = new VisualElement();
            m_DragLine.name = "splitterDragline";
            var resizer = new SquareResizer(LeftPane);
            m_DragLine.AddManipulator(resizer);
            resizer.LeftPaneWidthChanged += (f) => LeftPaneWidthChanged(f);

            dragLineAnchor.Add(m_DragLine);

            RightPane = new VisualElement();
            RightPane.style.flexGrow = 1;
            Add(RightPane);
        }

        class SquareResizer : MouseManipulator
        {
            Vector2 m_Start;
            protected bool m_Active;
            VisualElement m_LeftPane;

            public event Action<float> LeftPaneWidthChanged = delegate {};

            public SquareResizer(VisualElement leftPane)
            {
                m_LeftPane = leftPane;
                activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
                m_Active = false;
            }

            protected override void RegisterCallbacksOnTarget()
            {
                target.RegisterCallback<MouseDownEvent>(OnMouseDown);
                target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
                target.RegisterCallback<MouseUpEvent>(OnMouseUp);
            }

            protected override void UnregisterCallbacksFromTarget()
            {
                target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
                target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
                target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
            }

            protected void OnMouseDown(MouseDownEvent e)
            {
                if (m_Active)
                {
                    e.StopImmediatePropagation();
                    return;
                }

                if (CanStartManipulation(e))
                {
                    m_Start = e.localMousePosition;

                    m_Active = true;
                    target.CaptureMouse();
                    e.StopPropagation();
                }
            }

            protected void OnMouseMove(MouseMoveEvent e)
            {
                if (!m_Active || !target.HasMouseCapture())
                    return;

                Vector2 diff = e.localMousePosition - m_Start;

                m_LeftPane.style.width = m_LeftPane.layout.width + diff.x;

                if (diff.x != 0)
                    LeftPaneWidthChanged(m_LeftPane.layout.width);

                e.StopPropagation();
            }

            protected void OnMouseUp(MouseUpEvent e)
            {
                if (!m_Active || !target.HasMouseCapture() || !CanStopManipulation(e))
                    return;

                m_Active = false;
                target.ReleaseMouse();
                e.StopPropagation();
            }
        }
    }
}
