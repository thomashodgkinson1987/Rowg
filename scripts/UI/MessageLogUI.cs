using Godot;
using System.Collections.Generic;

namespace Rowg.UI
{

    public class MessageLogUI : Control
    {

        #region Enums

        public enum EScrollMode { NewestAtTop, NewestAtBottom }

        #endregion // Enums



        #region Nodes

        private ScrollContainer node_scrollContainer;
        private VBoxContainer node_vBoxContainer;

        #endregion // Nodes



        #region Fields

        [Export] private PackedScene m_messageLogEntryPackedScene;

        [Export] private int m_maxEntries = 32;

        [Export] private EScrollMode m_scrollMode = EScrollMode.NewestAtTop;

        [Export] private Color m_newestEntryColor = new Color(1, 1, 1);
        [Export] private Color m_oldestEntryColor = new Color(.3f, .3f, .3f);
        [Export] private int m_entryColorLerpCount = 4;

        private bool m_isDirty = false;

        private readonly List<RichTextLabel> m_entries;
        private readonly List<string> m_archive;

        #endregion // Fields



        #region Constructors

        public MessageLogUI () : base()
        {
            m_entries = new List<RichTextLabel>();
            m_archive = new List<string>();
        }

        #endregion // Constructors



        #region Node2D methods

        public override void _EnterTree ()
        {
            node_scrollContainer = GetNode<ScrollContainer>("Panel/ScrollContainer");
            node_vBoxContainer = GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer");
        }

        public override void _Ready ()
        {
            foreach (object obj in node_vBoxContainer.GetChildren())
            {
                if (obj is RichTextLabel entry)
                {
                    m_archive.Add(entry.Text);

                    if (m_scrollMode == EScrollMode.NewestAtTop)
                        m_entries.Insert(0, entry);
                    else if (m_scrollMode == EScrollMode.NewestAtBottom)
                        m_entries.Add(entry);
                }
            }
        }

        public override void _Process (float delta)
        {
            if (m_isDirty)
            {
                m_isDirty = false;
                node_scrollContainer.ScrollVertical = (int)node_scrollContainer.GetVScrollbar().MaxValue;
            }
        }

        #endregion // Node2D methods



        #region Public methods

        public void AddEntry (string message)
        {
            m_archive.Add(message);

            RichTextLabel entry = m_messageLogEntryPackedScene.Instance<RichTextLabel>();
            entry.BbcodeText = message;

            node_vBoxContainer.AddChild(entry);

            switch (m_scrollMode)
            {
                case EScrollMode.NewestAtTop:
                    node_vBoxContainer.MoveChild(entry, 0);
                    m_entries.Insert(0, entry);
                    if (m_entries.Count > m_maxEntries)
                    {
                        node_vBoxContainer.RemoveChild(m_entries[m_entries.Count - 1]);
                        m_entries[m_entries.Count - 1].QueueFree();
                        m_entries.RemoveAt(m_entries.Count - 1);
                    }
                    break;

                case EScrollMode.NewestAtBottom:
                    m_isDirty = true;
                    m_entries.Add(entry);
                    if (m_entries.Count > m_maxEntries)
                    {
                        node_vBoxContainer.RemoveChild(m_entries[0]);
                        m_entries[0].QueueFree();
                        m_entries.RemoveAt(0);
                    }
                    break;

                default:
                    node_vBoxContainer.MoveChild(entry, 0);
                    m_entries.Insert(0, entry);
                    if (m_entries.Count > m_maxEntries)
                    {
                        node_vBoxContainer.RemoveChild(m_entries[m_entries.Count - 1]);
                        m_entries[m_entries.Count - 1].QueueFree();
                        m_entries.RemoveAt(m_entries.Count - 1);
                    }
                    break;
            }

            UpdateEntryColors();
        }

        #endregion // Public methods



        #region Private methods

        private void UpdateEntryColors ()
        {
            switch (m_scrollMode)
            {
                case EScrollMode.NewestAtTop:
                    for (int i = 0; i < m_entries.Count; i++)
                    {
                        if (i <= m_entryColorLerpCount)
                            m_entries[i].SelfModulate = m_newestEntryColor.LinearInterpolate(m_oldestEntryColor, (float)i / m_entryColorLerpCount);
                        else
                            m_entries[i].SelfModulate = m_oldestEntryColor;
                    }
                    break;

                case EScrollMode.NewestAtBottom:
                    for (int i = m_entries.Count - 1; i >= 0; i--)
                    {
                        if (i >= (m_entries.Count - 1) - m_entryColorLerpCount)
                            m_entries[i].SelfModulate = m_newestEntryColor.LinearInterpolate(m_oldestEntryColor, (float)(m_entries.Count - 1 - i) / m_entryColorLerpCount);
                        else
                            m_entries[i].SelfModulate = m_oldestEntryColor;
                    }
                    break;

                default:
                    for (int i = 0; i < m_entries.Count; i++)
                    {
                        if (i <= m_entryColorLerpCount)
                            m_entries[i].SelfModulate = m_newestEntryColor.LinearInterpolate(m_oldestEntryColor, (float)i / m_entryColorLerpCount);
                        else
                            m_entries[i].SelfModulate = m_oldestEntryColor;
                    }
                    break;
            }
        }

        #endregion Private methods

    }

}
