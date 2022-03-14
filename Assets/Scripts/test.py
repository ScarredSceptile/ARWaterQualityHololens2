import cv2 as cv
import numpy as np

# Color of a lake [blue green red]
BGR = np.array([255, 218, 170])
upper = BGR + 10
lower = BGR - 10

image = cv.imread("pond.png")
#cv.imshow('hjelp', image)
#key = cv.waitKey(0)
mask = cv.inRange(image, lower, upper)
#cv.imshow('hjelp', mask)
#key = cv.waitKey(0)

#contours = find_contours(mask)
contours, hierarchy = cv.findContours(mask.copy(), cv.RETR_EXTERNAL, cv.CHAIN_APPROX_SIMPLE)
#print(contours)
copy = list(contours)
copy.sort(key=len, reverse=True)
main_contour = copy[0] #get_main_contour(contours)

cv.drawContours(image, [main_contour], -1, (0, 0, 255), 2)
cv.imshow("contours", image)

key = cv.waitKey(0)