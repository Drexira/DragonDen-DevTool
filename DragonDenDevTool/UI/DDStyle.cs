using UnityEngine;

namespace DragonDenDevTool.UI;

public sealed class DDStyle
{
  public GUIStyle Win;
  public GUIStyle Label;
  public GUIStyle LabelWarning;
  public GUIStyle SmallLabel;
  public GUIStyle Muted;
  public GUIStyle Button;
  public GUIStyle BtnDanger;
  public GUIStyle TabOn;
  public GUIStyle TabOff;
  public GUIStyle TextBox;
  public GUIStyle SearchBox;
  public GUIStyle ColumnHeader;
  public GUIStyle PopupBg;
  public GUIStyle HeaderBar;
  public GUIStyle Pill;
  public GUIStyle H1;
  public GUIStyle H2;
  public GUIStyle Separator;

  bool _built;

  Texture2D _txWin;
  Texture2D _txHeader;
  Texture2D _txPanel;
  Texture2D _txPill;

  Texture2D _txBtnN;
  Texture2D _txBtnH;
  Texture2D _txBtnA;
  Texture2D _txBtnF;
  Texture2D _txBtnOnN;
  Texture2D _txBtnOnH;
  Texture2D _txBtnOnA;
  Texture2D _txBtnOnF;
  Texture2D _txBtnDangerA;

  Texture2D _txTabOffN;
  Texture2D _txTabOffH;
  Texture2D _txTabOffA;
  Texture2D _txTabOn;

  Texture2D _txTextN;
  Texture2D _txTextH;
  Texture2D _txTextF;

  public void BuildIfNeeded()
  {
    if (_built) return;
    _built = true;

    var bg = new Color(0.10f, 0.10f, 0.11f, 1f);
    var panel = new Color(0.13f, 0.13f, 0.15f, 1f);
    var panelHi = new Color(0.16f, 0.16f, 0.18f, 1f);
    var panelLo = new Color(0.11f, 0.11f, 0.12f, 1f);
    var accent = new Color(1.00f, 0.55f, 0.15f, 1f);
    var accentDark = new Color(0.90f, 0.45f, 0.10f, 1f);
    var text = new Color(0.92f, 0.92f, 0.94f, 1f);
    var muted = new Color(0.70f, 0.70f, 0.72f, 1f);
    var border = new Color(1f, 1f, 1f, 0.06f);
    var borderStrong = new Color(1f, 1f, 1f, 0.12f);
    var focusRing = new Color(1f, 1f, 1f, 0.20f);
    var danger = new Color(0.92f, 0.25f, 0.25f, 1f);

    GUI.skin.settings.cursorColor = text;
    GUI.skin.settings.selectionColor = new Color(accent.r, accent.g, accent.b, 0.35f);

    _txWin = RoundedGrad(12, 12, bg, new Color(0.08f, 0.08f, 0.09f, 1f), borderStrong, 10, 1, 220, true);
    _txHeader = RoundedGrad(12, 12, new Color(0.12f, 0.12f, 0.14f, 1f), new Color(0.10f, 0.10f, 0.12f, 1f), borderStrong, 8, 1, 180, false);
    _txPanel = RoundedGrad(12, 12, panel, panelLo, border, 10, 1, 160, false);
    _txPill = RoundedFill(12, 12, new Color(1f, 1f, 1f, 0.06f), border, 10, 1);

    _txBtnN = RoundedGrad(12, 12, new Color(0.19f, 0.19f, 0.22f, 1f), new Color(0.16f, 0.16f, 0.18f, 1f), border, 8, 1, 180, false);
    _txBtnH = RoundedGrad(12, 12, new Color(0.24f, 0.24f, 0.27f, 1f), new Color(0.20f, 0.20f, 0.22f, 1f), borderStrong, 8, 1, 180, false);
    _txBtnA = RoundedGrad(12, 12, accent, accentDark, new Color(0f, 0f, 0f, 0.15f), 8, 1, 200, false);
    _txBtnF = RoundedGrad(12, 12, new Color(0.24f, 0.24f, 0.27f, 1f), new Color(0.20f, 0.20f, 0.22f, 1f), focusRing, 8, 2, 180, false);

    _txBtnOnN = _txBtnA;
    _txBtnOnH = RoundedGrad(12, 12, accent, accentDark, new Color(1f, 1f, 1f, 0.18f), 8, 2, 210, false);
    _txBtnOnA = RoundedGrad(12, 12, accentDark, accentDark, new Color(0f, 0f, 0f, 0.20f), 8, 2, 210, false);
    _txBtnOnF = RoundedGrad(12, 12, accent, accentDark, new Color(1f, 1f, 1f, 0.25f), 8, 2, 210, false);

    _txBtnDangerA = RoundedGrad(12, 12, danger, new Color(0.65f, 0.18f, 0.18f, 1f), new Color(0f, 0f, 0f, 0.15f), 8, 1, 200, false);

    _txTabOffN = RoundedGrad(12, 12, new Color(0.18f, 0.18f, 0.20f, 1f), new Color(0.15f, 0.15f, 0.17f, 1f), border, 8, 1, 180, false);
    _txTabOffH = RoundedGrad(12, 12, new Color(0.22f, 0.22f, 0.24f, 1f), new Color(0.18f, 0.18f, 0.20f, 1f), borderStrong, 8, 1, 180, false);
    _txTabOffA = RoundedGrad(12, 12, new Color(0.24f, 0.24f, 0.26f, 1f), new Color(0.20f, 0.20f, 0.22f, 1f), borderStrong, 8, 1, 180, false);
    _txTabOn = RoundedGrad(12, 12, accent, accentDark, new Color(0f, 0f, 0f, 0.15f), 8, 1, 200, false);

    _txTextN = RoundedGrad(12, 12, panel, panelLo, border, 10, 1, 140, false);
    _txTextH = RoundedGrad(12, 12, panelHi, panel, borderStrong, 10, 1, 160, false);
    _txTextF = RoundedGrad(12, 12, panelHi, panel, focusRing, 10, 2, 160, false);

    Win = new GUIStyle(GUI.skin.window);
    Win.normal.background = _txWin;
    Win.active.background = _txWin;
    Win.focused.background = _txWin;
    Win.onNormal.background = _txWin;
    Win.onActive.background = _txWin;
    Win.onFocused.background = _txWin;
    Win.normal.textColor = text;
    Win.padding = new RectOffset(10, 10, 10, 10);
    Win.border = new RectOffset(8, 8, 8, 8);

    Label = new GUIStyle(GUI.skin.label);
    Label.normal.textColor = text;
    Label.fontSize = 13;

    LabelWarning = new GUIStyle(GUI.skin.label);
    LabelWarning.normal.textColor = danger;
    LabelWarning.fontSize = 14;

    SmallLabel = new GUIStyle(Label);
    SmallLabel.fontSize = 12;
    SmallLabel.normal.textColor = muted;

    Muted = new GUIStyle(SmallLabel);

    Button = new GUIStyle(GUI.skin.button);
    Button.alignment = TextAnchor.MiddleCenter;
    Button.border = new RectOffset(8, 8, 8, 8);
    Button.padding = new RectOffset(10, 10, 6, 6);
    Button.margin = new RectOffset(2, 2, 2, 2);

    Button.normal.background = _txBtnN;
    Button.hover.background = _txBtnH;
    Button.active.background = _txBtnA;
    Button.focused.background = _txBtnF;
    Button.normal.textColor = text;
    Button.hover.textColor = text;
    Button.active.textColor = Color.black;
    Button.focused.textColor = text;

    Button.onNormal.background = _txBtnOnN;
    Button.onHover.background = _txBtnOnH;
    Button.onActive.background = _txBtnOnA;
    Button.onFocused.background = _txBtnOnF;
    Button.onNormal.textColor = Color.black;
    Button.onHover.textColor = Color.black;
    Button.onActive.textColor = Color.black;
    Button.onFocused.textColor = Color.black;

    BtnDanger = new GUIStyle(Button);
    BtnDanger.active.background = _txBtnDangerA;
    BtnDanger.onActive.background = _txBtnDangerA;

    TabOff = new GUIStyle(GUI.skin.button);
    TabOff.normal.textColor = Label.normal.textColor;
    TabOff.normal.background = _txTabOffN;
    TabOff.hover.background = _txTabOffH;
    TabOff.active.background = _txTabOffA;
    TabOff.focused.background = _txTabOffH;
    TabOff.border = new RectOffset(8, 8, 8, 8);
    TabOff.padding = new RectOffset(12, 12, 6, 6);
    TabOff.margin = new RectOffset(2, 2, 2, 2);

    TabOn = new GUIStyle(TabOff);
    TabOn.normal.textColor = Color.black;
    TabOn.normal.background = _txTabOn;
    TabOn.hover.background = _txTabOn;
    TabOn.active.background = _txTabOn;
    TabOn.focused.background = _txTabOn;

    TextBox = new GUIStyle(GUI.skin.textField);
    TextBox.normal.textColor = text;
    TextBox.normal.background = _txTextN;
    TextBox.hover.background = _txTextH;
    TextBox.active.background = _txTextH;
    TextBox.focused.background = _txTextF;
    TextBox.border = new RectOffset(8, 8, 8, 8);
    TextBox.padding = new RectOffset(8, 8, 6, 6);

    SearchBox = new GUIStyle(TextBox);

    ColumnHeader = new GUIStyle(Label);
    ColumnHeader.fontStyle = FontStyle.Bold;
    ColumnHeader.normal.textColor = accent;

    PopupBg = new GUIStyle(GUI.skin.box);
    PopupBg.normal.background = _txPanel;
    PopupBg.border = new RectOffset(10, 10, 10, 10);
    PopupBg.padding = new RectOffset(8, 8, 8, 8);

    HeaderBar = new GUIStyle(GUI.skin.box);
    HeaderBar.normal.background = _txHeader;
    HeaderBar.normal.textColor = accent;
    HeaderBar.alignment = TextAnchor.MiddleLeft;
    HeaderBar.padding = new RectOffset(10, 10, 6, 6);
    HeaderBar.border = new RectOffset(8, 8, 8, 8);

    Pill = new GUIStyle(GUI.skin.box);
    Pill.normal.background = _txPill;
    Pill.normal.textColor = text;
    Pill.padding = new RectOffset(8, 8, 6, 6);
    Pill.margin = new RectOffset(0, 0, 2, 2);
    Pill.border = new RectOffset(8, 8, 8, 8);

    H1 = new GUIStyle(Label);
    H1.fontSize = 16;
    H1.fontStyle = FontStyle.Bold;
    H1.margin = new RectOffset(4, 4, 6, 2);

    H2 = new GUIStyle(Label);
    H2.fontSize = 13;
    H2.fontStyle = FontStyle.Bold;
    H2.normal.textColor = accent;
    H2.margin = new RectOffset(4, 4, 6, 2);

    Separator = new GUIStyle(GUI.skin.box);
    Separator.normal.background = MakeLineTex(new Color(1f, 1f, 1f, 0.10f), new Color(0f, 0f, 0f, 0.25f));
    Separator.fixedHeight = 2f;
    Separator.margin = new RectOffset(0, 0, 6, 6);
  }

  static Texture2D MakeLineTex(Color top, Color bottom)
  {
    var tex = new Texture2D(1, 2, TextureFormat.RGBA32, false);
    tex.SetPixel(0, 0, top);
    tex.SetPixel(0, 1, bottom);
    tex.Apply(false, true);
    return tex;
  }

  static Texture2D RoundedFill(int w, int h, Color fill, Color border, int radius, int borderW)
  {
    var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
    var px = tex.GetPixels32();
    var cFill = (Color32)fill;
    var cBorder = (Color32)border;

    var bw = Mathf.Clamp(borderW, 0, 8);
    var r = Mathf.Clamp(radius, 0, 32);

    for (var y = 0; y < h; y++)
    {
      for (var x = 0; x < w; x++)
      {
        var inside = InsideRoundRect(x, y, w, h, r);
        var onBorder = bw > 0 && inside && !InsideRoundRect(x, y, w, h, Mathf.Max(r - bw, 0));
        px[y * w + x] = onBorder ? cBorder : (inside ? cFill : new Color32(0, 0, 0, 0));
      }
    }

    tex.SetPixels32(px);
    tex.Apply(false, true);
    tex.wrapMode = TextureWrapMode.Clamp;
    tex.filterMode = FilterMode.Bilinear;
    return tex;
  }

  static Texture2D RoundedGrad(int w, int h, Color top, Color bottom, Color border, int radius, int borderW, byte gloss, bool dropShadow)
  {
    var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
    var px = tex.GetPixels32();
    var r = Mathf.Clamp(radius, 0, 32);
    var bw = Mathf.Clamp(borderW, 0, 8);

    for (int y = 0; y < h; y++)
    {
      var t = (float)y / Mathf.Max(1, h - 1);
      var grad = Color.Lerp(bottom, top, t);
      var glossAlpha = (gloss / 255f) * 0.15f * Mathf.Clamp01(1f - Mathf.Abs(t - 0.25f) * 3f);
      var fill = Color.Lerp(grad, Color.white, glossAlpha);

      for (int x = 0; x < w; x++)
      {
        var inside = InsideRoundRect(x, y, w, h, r);
        var onBorder = bw > 0 && inside && !InsideRoundRect(x, y, w, h, Mathf.Max(r - bw, 0));

        Color col = onBorder ? border : fill;
        if (!inside)
        {
          col.a = 0f;

          if (dropShadow)
          {
            int sx = x - 1;
            int sy = y + 1;
            if (InsideRoundRect(sx, sy, w, h, r))
              col = new Color(0f, 0f, 0f, 0.25f);
          }
        }

        px[y * w + x] = col;
      }
    }

    tex.SetPixels32(px);
    tex.Apply(false, true);
    tex.wrapMode = TextureWrapMode.Clamp;
    tex.filterMode = FilterMode.Bilinear;
    return tex;
  }

  static bool InsideRoundRect(int x, int y, int w, int h, int r)
  {
    if (r <= 0) return x >= 0 && x < w && y >= 0 && y < h;
    var dx = Mathf.Min(x, w - 1 - x);
    var dy = Mathf.Min(y, h - 1 - y);
    if (dx >= r || dy >= r) return x >= 0 && x < w && y >= 0 && y < h;
    var rx = r - dx;
    var ry = r - dy;
    return (rx * rx + ry * ry) <= r * r;
  }
}