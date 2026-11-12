"""Generates the Castaway icon: src/Castaway.Desktop/Assets/castaway.ico + .png and assets/castaway-256.png / -512.png.
Run: python tools/make_icon.py   (needs Pillow)

A charcoal rounded tile with an amber play button riding on two waves (cast away, out to sea).
"""
from PIL import Image, ImageDraw, ImageFilter
import math, os

S = 1024
here = os.path.dirname(os.path.abspath(__file__))


def lerp(a, b, t):
    t = max(0.0, min(1.0, t))
    return tuple(int(a[i] + (b[i] - a[i]) * t) for i in range(len(a)))


def vertical_gradient(size, top, bottom):
    img = Image.new("RGBA", (size, size))
    d = ImageDraw.Draw(img)
    for y in range(size):
        d.line([(0, y), (size, y)], fill=lerp(top, bottom, y / size))
    return img


img = Image.new("RGBA", (S, S), (0, 0, 0, 0))
pad, radius = 64, 210
tile_box = [pad, pad, S - pad, S - pad]

# Soft shadow under the tile
shadow = Image.new("RGBA", (S, S), (0, 0, 0, 0))
ImageDraw.Draw(shadow).rounded_rectangle([pad + 10, pad + 34, S - pad - 10, S - pad + 18], radius, fill=(0, 0, 0, 130))
img.alpha_composite(shadow.filter(ImageFilter.GaussianBlur(26)))

# Tile: charcoal, lighter at the top
tile_mask = Image.new("L", (S, S), 0)
ImageDraw.Draw(tile_mask).rounded_rectangle(tile_box, radius, fill=255)
img.paste(vertical_gradient(S, (52, 56, 66, 255), (16, 17, 21, 255)), (0, 0), tile_mask)

# Thin top highlight along the tile edge
rim = Image.new("RGBA", (S, S), (0, 0, 0, 0))
ImageDraw.Draw(rim).rounded_rectangle(tile_box, radius, outline=(255, 255, 255, 40), width=6)
img.alpha_composite(rim)

amber_hi, amber_lo = (255, 214, 120, 255), (229, 150, 28, 255)

# Play triangle (rounded corners by drawing it big and blurring the mask slightly)
cx, cy = S / 2 + 26, S / 2 - 70
h = 330
tri = [(cx - h * 0.45, cy - h / 2), (cx - h * 0.45, cy + h / 2), (cx + h * 0.55, cy)]
tri_mask = Image.new("L", (S, S), 0)
ImageDraw.Draw(tri_mask).polygon(tri, fill=255)
tri_mask = tri_mask.filter(ImageFilter.GaussianBlur(10)).point(lambda v: 255 if v > 128 else 0)
tri_shadow = Image.new("RGBA", (S, S), (0, 0, 0, 0))
tri_shadow.paste((0, 0, 0, 150), (0, 0), tri_mask)
img.alpha_composite(tri_shadow.filter(ImageFilter.GaussianBlur(16)).transform((S, S), Image.AFFINE, (1, 0, 0, 0, 1, -14)))
img.paste(vertical_gradient(S, amber_hi, amber_lo), (0, 0), tri_mask)


def wave(y0, amp, wavelength, width, color, phase=0.0):
    # Stamp a disc every pixel along the curve: a smooth band with round ends.
    mask = Image.new("L", (S, S), 0)
    d = ImageDraw.Draw(mask)
    r = width / 2
    for x in range(pad + 70, S - pad - 70 + 1):
        y = y0 + amp * math.sin(2 * math.pi * (x - pad) / wavelength + phase)
        d.ellipse([x - r, y - r, x + r, y + r], fill=255)
    layer = Image.new("RGBA", (S, S), (0, 0, 0, 0))
    layer.paste(Image.new("RGBA", (S, S), color[:3] + (255,)), (0, 0), mask.point(lambda v: v * color[3] // 255))
    return layer


img.alpha_composite(wave(S / 2 + 190, 26, 300, 46, amber_lo, 0.6))
img.alpha_composite(wave(S / 2 + 276, 22, 300, 34, (229, 150, 28, 150), 2.2))

assets = os.path.join(here, "..", "src", "Castaway.Desktop", "Assets")
os.makedirs(assets, exist_ok=True)
img.resize((256, 256), Image.LANCZOS).save(os.path.join(assets, "castaway.png"))
img.save(os.path.join(assets, "castaway.ico"),
         sizes=[(16, 16), (20, 20), (24, 24), (32, 32), (40, 40), (48, 48), (64, 64), (128, 128), (256, 256)])
root_assets = os.path.join(here, "..", "assets")
os.makedirs(root_assets, exist_ok=True)
img.resize((256, 256), Image.LANCZOS).save(os.path.join(root_assets, "castaway-256.png"))
img.resize((512, 512), Image.LANCZOS).save(os.path.join(root_assets, "castaway-512.png"))
print("ok")
