# skinet
The project used from the course TryCatchLearn

## Table Of Contents

- [Technologies](#technologies)
- [Configurations](#configurations)  
  - [Chocolatey](#chocolatey)
  - [NodeJs](#nodeJs)
  - [HTTPS](#https)

## Technologies
Technologies used in this course:

* .Net 7
  * Entity Framework SQLite 7.0.0
  * AutoMapper 12.0.1
* Angular 16.2.0
* NodeJs 16.14.0

**Optional**
> Only to make it easy to install and configure

* NVM
* Chocolatey
* MKCERT

## Configurations
Configurations that I use to configure and run these applications

### Chocolatey
The main reason to install Chocolatey it's because NVM is not supported on Windows.  Chocolatey it's a package manager for Windows.

So, nothing better than following the official documentation.
- [Installing Chocolatey](https://chocolatey.org/install)

### NodeJs
Let's use Chocolatey to install and manage the Node versions. Open Powershell as administrator and running this:

``` powershell
# chose desired node version
$version = "8.12.0"
# install nvm w/ cmder
choco install cmder
choco install nvm
refreshenv
# install node
nvm install $version
nvm use $version
```

> [!NOTE]
> You may have a problem executing `refreshenv`, on the console, they'll give you the command necessary to run.

### HTTPS

It's necessary a certificate to run the Angular application, we'll use MKCERT to create a `localhost` certificate.

First, install MKCERT
``` powershell
choco install mkcert
```

Navigate to folder "/client/ssl" and create the certificate.

``` powershell
mkcert -install
```

Then add `localhost` on the certificate

``` powershell
mkcert localhost
```

> [!NOTE]
> It's necessary to execute this process on different machines that need to be configured.

