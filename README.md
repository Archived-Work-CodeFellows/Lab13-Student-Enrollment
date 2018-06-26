# Lab13-Student-Enrollment  

This project is more practice in creating a full CRUD MVC application as well as a deployed version through Azure Services. This was built with an empty web app in Visual Studio and uses .Net core 2.1 sdk.

On this site, you can enroll students to specific courses, create new students and courses as well as remove and update students and courses. It also implements a search feature on the Students page and Courses page for finding similar or specific results.

***
### Getting Started:
Visit the deployed site [here](http://eisjlab13.azurewebsites.net/) to see a working version. If that is no longer available, follow these steps to deploy it locally

* Download the latest .NET SDK
* Navigate and dowload the project files
* When you open, you'll want to make sure that you create a local database for CRUD to work properly. In the PM console, use `Update-Database` as it will take the existing migrations with the project and create a new setup for you
  * (note: sometimes it will say install EF core and sometimes it does it for you)
* There is a SeedData class to help fill in some entries for your newly created database
* After that is completed, run the application. If you see the bottom images then success! You can now explore the Student Enrollment Full CRUD MVC application.
***
### Visuals (taken from deployed)
* Home page
![Home Page](/home-landing.PNG)
* Students and Courses landings

![Students Page](/students-landing.PNG)
![Course Page](/courses-landing.PNG)

* Student and Course Details
![Student Detail](/student-details.PNG)
![Course Detail](/course-details.PNG)

***