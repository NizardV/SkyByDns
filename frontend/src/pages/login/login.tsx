import { useState } from 'react';
import { useNavigate } from 'react-router-dom'; // Changed from redirect
import { useAuth } from '@/contexts/AuthContext';
import { toast } from 'sonner';
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import type { LoginCredentials, RegisterData } from '@/types/auth'
import Spinner from '@/components/ui/spinner';

export function Login() {
  const { login, register, isLoading: authIsLoading } = useAuth();
  const navigate = useNavigate();
  
  // Local loading state for form submission
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [activeTab, setActiveTab] = useState<'login' | 'register'>('login');
  
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [email, setEmail] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');

  const handleChangePassword = (event: React.ChangeEvent<HTMLInputElement>) => setPassword(event.target.value);
  const handleChangeConfirmPassword = (event: React.ChangeEvent<HTMLInputElement>) => setConfirmPassword(event.target.value);
  const handleChangeEmail = (event: React.ChangeEvent<HTMLInputElement>) => setEmail(event.target.value);
  const handleChangeFirstName = (event: React.ChangeEvent<HTMLInputElement>) => setFirstName(event.target.value);
  const handleChangeLastName = (event: React.ChangeEvent<HTMLInputElement>) => setLastName(event.target.value);

  const resetForm = () => {
    setPassword('');
    setConfirmPassword('');
    setEmail('');
    setFirstName('');
    setLastName('');
  };

  const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsSubmitting(true);

    const credentials: LoginCredentials = {
      email: email,
      password: password,
    };

    try {
      const response = await login(credentials);
      if (response.success) {
        toast.success('Login successful!');
        resetForm();
        navigate('/dashboard');
      } else {
        // Error is already handled by the auth context/login function
        // Just reset the password fields for security
        setPassword('');
      }
    } catch (err) {
      toast.error(err.message || 'Login failed');
      setPassword('');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleRegister = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsSubmitting(true);

    // Validate passwords match
    if (password !== confirmPassword) {
      toast.error('Passwords do not match');
      setIsSubmitting(false);
      return;
    }

    const userInfo: RegisterData = {
      firstName: firstName,
      lastName: lastName,
      email: email,
      password: password,
    };

    try {
      const response = await register(userInfo);

      if (response.success) {
        toast.success('Registration successful!');
        resetForm();
        // Switch to login tab after successful registration
        setActiveTab('login');
        navigate('/dashboard');
      } else {
        // Reset password fields for security
        setPassword('');
        setConfirmPassword('');
      }
    } catch (err) {
      toast.error(err.message || 'Registration failed');
      setPassword('');
      setConfirmPassword('');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleTabChange = (value: string) => {
    setActiveTab(value as 'login' | 'register');
    // Reset form when switching tabs
    resetForm();
  };

  // Combined loading state
  const isLoading = authIsLoading || isSubmitting;

  return (
    <div className="flex w-full max-w-sm flex-col gap-6">
      <Tabs value={activeTab} onValueChange={handleTabChange}>
        <TabsList className="grid w-full grid-cols-2">
          <TabsTrigger value="login">Login</TabsTrigger>
          <TabsTrigger value="register">Register</TabsTrigger>
        </TabsList>
        
        <TabsContent value="login">
          <form onSubmit={handleLogin}>
            <Card>
              <CardHeader>
                <CardTitle>Login</CardTitle>
                <CardDescription>
                  Enter your email and password to login to your account.
                </CardDescription>
              </CardHeader>
              <CardContent className="grid gap-6">
                <div className="grid gap-3">
                  <Label htmlFor="login-email">Email</Label>
                  <Input
                    id="login-email"
                    name="email"
                    type="email"
                    value={email}
                    onChange={handleChangeEmail}
                    required
                    disabled={isLoading}
                  />
                </div>
                <div className="grid gap-3">
                  <Label htmlFor="login-password">Password</Label>
                  <Input
                    id="login-password"
                    name="password"
                    type="password"
                    value={password}
                    onChange={handleChangePassword}
                    required
                    disabled={isLoading}
                  />
                </div>
              </CardContent>
              <CardFooter>
                <Button
                  type="submit"
                  className="w-full"
                  disabled={isLoading}
                >
                  {isLoading ? <Spinner /> : 'Login'}
                </Button>
              </CardFooter>
            </Card>
          </form>
        </TabsContent>
        
        <TabsContent value="register">
          <form onSubmit={handleRegister}>
            <Card>
              <CardHeader>
                <CardTitle>Create Account</CardTitle>
                <CardDescription>
                  Enter your information to create a new account.
                </CardDescription>
              </CardHeader>
              <CardContent className="grid gap-6">
                <div className="flex flex-row gap-3">
                  <div className="flex-1">
                    <Label htmlFor="first-name" className='mb-2 block'>First name</Label>
                    <Input
                      id="first-name"
                      name="first-name"
                      type="text"
                      value={firstName}
                      onChange={handleChangeFirstName}
                      required
                      disabled={isLoading}
                    />
                  </div>
                  <div className="flex-1">
                    <Label htmlFor="last-name" className='mb-2 block'>Last name</Label>
                    <Input
                      id="last-name"
                      name="last-name"
                      type="text"
                      value={lastName}
                      onChange={handleChangeLastName}
                      required
                      disabled={isLoading}
                    />
                  </div>
                </div>
                <div className="grid gap-3">
                  <Label htmlFor="register-email">Email</Label>
                  <Input
                    id="register-email"
                    name="email"
                    type="email"
                    value={email}
                    onChange={handleChangeEmail}
                    required
                    disabled={isLoading}
                  />
                </div>
                <div className="grid gap-3">
                  <Label htmlFor="new-password">Password</Label>
                  <Input
                    id="new-password"
                    name="new-password"
                    type="password"
                    value={password}
                    onChange={handleChangePassword}
                    required
                    disabled={isLoading}
                  />
                </div>
                <div className="grid gap-3">
                  <Label htmlFor="confirm-password">Confirm Password</Label>
                  <Input
                    id="confirm-password"
                    name="confirm-password"
                    type="password"
                    value={confirmPassword}
                    onChange={handleChangeConfirmPassword}
                    required
                    disabled={isLoading}
                  />
                </div>
              </CardContent>
              <CardFooter>
                <Button
                  type="submit"
                  className="w-full"
                  disabled={isLoading}
                >
                  {isLoading ? <Spinner /> : 'Create Account'}
                </Button>
              </CardFooter>
            </Card>
          </form>
        </TabsContent>
      </Tabs>
    </div>
  );
}