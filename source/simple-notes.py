from pathlib import Path
import os
import tkinter as tk
from tkinter.font import Font

root_dir = Path(os.getcwd()).parent

root = tk.Tk()
root.wm_attributes("-topmost", 1)
root.minsize(160, 120)

bg_color = '#e6d9a5'
text_font = Font(family="Helvetica", size=14)
panel_font = Font(family="Helvetica", size=12)

movable_icon = tk.PhotoImage(file='{}\\resources\\movable_icon.png'.format(root_dir))
resize_icon = tk.PhotoImage(file='{}\\resources\\resize_icon.png'.format(root_dir))
close_icon = tk.PhotoImage(file='{}\\resources\\close_icon.png'.format(root_dir))

def move_window(event):
    root.geometry('+{0}+{1}'.format(event.x_root, event.y_root))

def resize_window(event):
    size_x = event.x_root - root.winfo_x()
    size_y = event.y_root - root.winfo_y()
    if (size_x > 0 and size_y > 0):
        root.geometry('{0}x{1}'.format(size_x, size_y))

root.overrideredirect(True) # turns off title bar, geometry
root.geometry('250x350+100+100') # set new geometry

# make a frame for the title bar
title_bar = tk.Frame(root, bg=bg_color, relief='flat')
drag_bar = tk.Canvas(title_bar, bg=bg_color, width=36, height=36, borderwidth=0, highlightthickness=0)
drag_bar.create_image(17, 17, image=movable_icon)
bottom_bar = tk.Frame(root, bg=bg_color)
resize_bar = tk.Canvas(bottom_bar, bg=bg_color, cursor='sizing', width=36, height=36, borderwidth=0, highlightthickness=0)
resize_bar.create_image(18, 18, image=resize_icon)

# put a close button on the title bar
close_button = tk.Button(title_bar, image=close_icon, command=root.destroy, relief='flat', bg=bg_color)

# a canvas for the main area of the window
window = tk.Text(root, bg=bg_color, height=0, bd=10)

window.configure(font=text_font, relief='flat')

# pack the widgets
title_bar.pack(fill=tk.X)
drag_bar.pack(side=tk.LEFT, fill='y')
close_button.pack(side=tk.RIGHT)
window.pack(expand=1, fill=tk.BOTH)
bottom_bar.pack(fill='x')
resize_bar.pack(side='right', fill='y', pady=0, padx=0, ipady=0, ipadx=0)

# bind title bar motion to the move window function
drag_bar.bind('<B1-Motion>', move_window)
resize_bar.bind('<B1-Motion>', resize_window)

root.mainloop()