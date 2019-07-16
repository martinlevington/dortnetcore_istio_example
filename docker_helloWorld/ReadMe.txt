Basic Steps
These are roughly the steps you need to follow to get an Istio enabled ‘HelloWorld’ app:

1. Create a Kubernetes cluster and install Istio with automatic sidecar injection.
2. Create a HelloWorld app in your language of choice, create a Docker image out of it and push it to a public image repository.
3. Create Kubernetes Deployment and Service for your container.
4. Create a Gateway to enable HTTP(S) traffic to your cluster.
5. Create a VirtualService to expose Kubernetes Service via Gateway.
6. (Optional) If you want to create multiple versions of your app, create a DestinationRule to define subsets that you can refer from the VirtualService.
7. (Optional) If you want to call external services outside your service mesh, create a ServiceEntry.





# build

docker build -t docker_hello_world .

# run

docker run -d -p 8080:80 --name helloworldapp docker_hello_world

# list images

docker images


# create yaml deploy and service files for kubernetes

# install helm from https://helm.sh/

# istio setup

#make sure you setup your platfporm 1st

https://istio.io/docs/setup/kubernetes/platform-setup/docker/

follow install instructions

https://istio.io/docs/setup/kubernetes/#downloading-the-release

e.g.

helm repo add istio.io https://storage.googleapis.com/istio-release/releases/1.2.2/charts/


curl -L https://git.io/getLatestIstio | ISTIO_VERSION=1.2.2 sh -

cd istio-1.2.2
export PATH=$PWD/bin:$PATH
kubectl apply -f install/kubernetes/helm/helm-service-account.yaml

# helm setup into the cluster

helm init --service-account tiller
kubectl create namespace istio-system
helm template install/kubernetes/helm/istio-init --name istio-init --namespace istio-system | kubectl apply -f -

- apply configuration profile

helm template install/kubernetes/helm/istio --name istio --namespace istio-system  --set kiali.enabled=true --set gateways.istio-ingressgateway.type=LoadBalancer --set tracing.enabled=true  --set grafana.enabled=true | kubectl apply -f -

- verify installation

kubectl get svc -n istio-system
kubectl get pods -n istio-system


-list the servces

kubectl get svc

- install the dashboard

kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.0.0-beta1/aio/deploy/recommended.yaml

- install a user (the following is a custom config file)
kubectl apply -f dashboard-adminuser.yaml
kubectl apply -f dashboard-rbac-authorization.yaml

- generate a token
kubectl -n kube-system describe secret $(kubectl -n kube-system get secret | grep admin-user | awk '{print $1}')

- dashboard url

http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/login

- run the followingto enabke access to the admin dash board
kubectl proxy

 .# auto sidecar injection

kubectl label namespace default istio-injection=enabled





# to deploy we need to create a deployment configuration file and setup the service 
# find the gateway to see if it is setup.

kubectl get svc -n istio-system -l istio=ingressgateway

example output:
NAME                   TYPE           CLUSTER-IP     EXTERNAL-IP   PORT(S)                                                                                                                                      AGE
istio-ingressgateway   LoadBalancer   10.98.228.44   localhost     15020:31913/TCP,80:31380/TCP,443:31390/TCP,31400:31400/TCP,15029:31220/TCP,15030:30382/TCP,15031:30623/TCP,15032:31867/TCP,15443:32493/TCP   1h

- we need to open the correct ports and then setup the routing inside kubernetes

# kiali

- configure the authentication for kiali

 kubectl apply -f kiali.yaml


kubectl -n istio-system get svc kiali

- forward the port

kubectl -n istio-system port-forward $(kubectl -n istio-system get pod -l app=kiali -o jsonpath='{.items[0].metadata.name}') 20001:20001


# to deploy

kubectl apply -f helloworld-deploy.yaml 
kubectl apply -f helloworld-service.yaml 
kubectl apply -f helloworld-gateway.yaml 
kubectl apply -f helloworld-virtualservice.yaml 

# to forward a port directly to a pod / port

kubectl port-forward     $(kubectl get pod -l app=helloworldapp -o jsonpath='{.items[0].metadata.name}')      8080:80





# Uninstall istio

helm delete --purge istio
helm delete --purge istio-init

# ping an internal service usi g busybox image

- note the name of the service is set in the service config
kubectl exec -ti busybox -- nslookup pingservice-service

# create a secret - called 'helloworld-appsettings' from a file

kubectl create secret generic helloworld-appsettings --from-file=./docker_helloWorld/appsettings.docker.json


 kubectl apply -f helloworld-deploy.yaml
 kubectl apply -f pingService-deploy.yaml