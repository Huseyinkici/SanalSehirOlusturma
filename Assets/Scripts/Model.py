import onnxruntime as ort
from PIL import Image
import numpy as np
import cv2
import time
class YOLOV8():
    def __init__(self,model_path):
        self.model = ort.InferenceSession(model_path, providers=['CUDAExecutionProvider'])
        self.running=False
        self.shape=640
        self.yolo_classes = [
        "bina"
        ]

    def preprocess(self,img):
        
        img_width, img_height = img.size
        img = img.resize((self.shape, self.shape))
        img = img.convert("RGB")
        input = np.array(img) / 255.0
        input = input.transpose(2, 0, 1)
        input = input.reshape(1, 3, self.shape, self.shape)
        return input.astype(np.float32), img_width, img_height


    def predictImage(self,image_path):
        img = Image.open(image_path)
        image,width,height=self.preprocess(img)
        outputs = self.model.run(["output0"], {"images":image})
        detected=self.process_output(outputs,width,height)
        return self.sendImage(detected,img)
    
    def predictVideoFrame(self,frame):
        org= cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        org=Image.fromarray(org)
        image,width,height=self.preprocess(org)
        outputs = self.model.run(["output0"], {"images":image})
        detected=self.process_output(outputs,width,height)
        return self.sendImage(detected,org)


    def sendImage(self, detected, original_image):
        original_image = np.array(original_image)
        original_image = cv2.cvtColor(original_image, cv2.COLOR_RGB2BGR)

        for box in detected:
            x1, y1, x2, y2, label, prob = box
            x1, y1, x2, y2 = int(x1), int(y1), int(x2), int(y2)
            cv2.rectangle(original_image, (x1, y1), (x2, y2), (0, 255, 0), 2)
            cv2.putText(original_image, f'{label}: {prob:.2f}', (x1, y1 - 5), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)


        original_image = cv2.cvtColor(original_image, cv2.COLOR_BGR2RGB)
        pil_image = Image.fromarray(original_image)

        with open(r"D:\DosyalarToplu\UnityProjects\BitirmeSehirOlusturmaYapayZeka\Assets\Scripts\cikti.txt", 'a') as file:
            file.write(str(len(detected))+",")
        return pil_image


    def process_output(self,output, img_width, img_height):

        output = output[0].astype(float)
        output = output.transpose() # satir to sutun, sutun to satir

        boxes = [] # algilanmis nesne bilgileri icin depo
        for row in output:
            prob = row[4:].max() # en buyuk degeri bul x>=5.stun
            if prob < 0.3:
                continue
            class_id = row[4:].argmax()
            label = self.yolo_classes[class_id]
            xc, yc, w, h = row[:4]
            x1 = (xc - w/2) / self.shape * img_width
            y1 = (yc - h/2) / self.shape * img_height
            x2 = (xc + w/2) / self.shape * img_width
            y2 = (yc + h/2) / self.shape * img_height
            boxes.append([x1, y1, x2, y2, label, prob])

        boxes.sort(key=lambda x: x[5], reverse=True)
        result = []
        while len(boxes) > 0:
            result.append(boxes[0])
            boxes = [box for box in boxes if self.iou(box, boxes[0]) < 0.5]

        return result

    def iou(self,box1,box2):

        return self.intersection(box1,box2)/self.union(box1,box2)


    def union(self,box1,box2):

        box1_x1,box1_y1,box1_x2,box1_y2 = box1[:4]
        box2_x1,box2_y1,box2_x2,box2_y2 = box2[:4]
        box1_area = (box1_x2-box1_x1)*(box1_y2-box1_y1)
        box2_area = (box2_x2-box2_x1)*(box2_y2-box2_y1)
        return box1_area + box2_area - self.intersection(box1,box2)


    def intersection(self,box1,box2):

        box1_x1,box1_y1,box1_x2,box1_y2 = box1[:4]
        box2_x1,box2_y1,box2_x2,box2_y2 = box2[:4]
        x1 = max(box1_x1,box2_x1)
        y1 = max(box1_y1,box2_y1)
        x2 = min(box1_x2,box2_x2)
        y2 = min(box1_y2,box2_y2)
        return (x2-x1)*(y2-y1)


    



 