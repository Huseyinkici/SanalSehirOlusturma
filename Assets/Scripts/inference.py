# inference.py
import argparse
import os
import sys
from Model import YOLOV8 as ml
from PIL import Image

paster=argparse.ArgumentParser()
paster.add_argument('imgPath')
paster.add_argument('outPath')
args=paster.parse_args()
image_path=args.imgPath
output_Path=args.outPath
# Path to the model
model_path = r"D:\DosyalarToplu\UnityProjects\BitirmeSehirOlusturmaYapayZeka\Assets\Scripts\best.onnx"

# Input and output folders
input_folder = "Screenshots/"
output_folder = "Results"

model = ml(model_path)
# Loop through images with numbers 0 to 15
for i in range(16):
    # Construct the image filename
    filename = f"{i}.png"
    img_path = os.path.join(image_path, filename)
    print(img_path)
    # Perform inference on the current image
    result = model.predictImage(img_path)

    # Display and save the result
    #result.show()
    result.save(os.path.join(output_Path, f"result_{filename}"))

print("Inference completed.")
